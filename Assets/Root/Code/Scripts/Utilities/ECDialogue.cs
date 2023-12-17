using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Linq;

//From Henrik Flink, Vision Trick Media.
public class ECDialogueLine
{
    public ECDialoguePage owner = null;
    public int speakerId = 0;
    public List<string[]> segmentSeperate = new List<string[]>();
    public string[] segments = null;
    public string[] segementCallback = null;
    public ECDialogueLine(int a_speakerId, int a_numOfSegments, ECDialoguePage a_owner)
    {
        speakerId = a_speakerId;
        segments = new string[a_numOfSegments];
        segementCallback = new string[a_numOfSegments];
        owner = a_owner;
    }
}

public class ECDialoguePage
{
    public ECDialogueContainer owner = null;
    private string[] data = null;
    public string id { get { return data[0]; } }
    public string toPage { get { return data.Length<2 ? "" : data[1]; } }
    public List<ECDialogueLine> lines = new List<ECDialogueLine>();
    public ECDialoguePage(string[] a_data, ECDialogueContainer a_owner)
    {
        data = a_data;
        owner = a_owner;
    }
}

public class ECDialogueContainer
{
    public ECDialoguePage startPage = null;
    public string id = "";
    public Dictionary<string, ECDialoguePage> pages = new Dictionary<string, ECDialoguePage>();
    public ECDialogueContainer(string a_id)
    {
        id = a_id;
    }
}

public class ECDialogue
{
    public static Dictionary<string, ECDialogueContainer> allContainers = new Dictionary<string, ECDialogueContainer>();

    public static ECDialogueLine nextLine(ECDialoguePage a_page, ref int a_counter, int a_segment=-1)
    {
        ECDialogueLine line = null;
        if(a_segment==-1)
        {
            if(a_counter>=a_page.lines.Count)
            {
                a_counter = 0;
                if (a_page.owner.pages.ContainsKey(a_page.toPage))
                    a_page = a_page.owner.pages[a_page.toPage];
                else
                    return null;
            }

            line = a_page.lines[a_counter];
        }
        // handle choice
        else if(a_page.owner.pages.ContainsKey(a_page.lines[a_counter-1].segementCallback[a_segment]))
        {
            a_page = a_page.owner.pages[a_page.lines[a_counter-1].segementCallback[a_segment]];
            a_counter = 0;
            line = a_page.lines[a_counter];
        }
        return line;
    }

    public static ECDialogueContainer parseAssetToDialogue(TextAsset a_asset)
    {
        string line;
        string[] data = a_asset.text.Split("\n"[0]);
        ECDialogueContainer newContainer = new ECDialogueContainer(new string(data[0].Replace("\t", "").Where(c => !char.IsControl(c)).ToArray()));
        ECDialoguePage currentPage = null;

        for(int i=1; i<data.Length; i++)
        {
            line = new string(data[i].Replace("\t", "").Where(c => !char.IsControl(c)).ToArray()); ;
            if(line!=null&&line.Length>1)
            {
                if(currentPage==null)
                {
                    currentPage = new ECDialoguePage(line.Split(':'), newContainer);
                    if (newContainer.startPage == null)
                        newContainer.startPage = currentPage;
                }
                else
                {
                    int startIndex = line.IndexOf('[');
                    int endIndex = line.IndexOf(']');
                    string[] segments = line.Substring(endIndex+1, (line.Length - (endIndex+1))).Split('|');
                    string ss = line.Substring(startIndex+1, (endIndex - startIndex)-1);
                    ECDialogueLine newLine = new ECDialogueLine(int.Parse(ss), segments.Length, currentPage);
                    for (int s=0;s<segments.Length; s++)
                    {
                        string[] segmentData = segments[s].Split(':');
                        newLine.segments[s] = segmentData[0];
                        newLine.segementCallback[s] = segmentData.Length > 1 ? segmentData[1] : "";
                        newLine.segmentSeperate.Add(segmentData[0].Split(' '));

                    }
                    
                    currentPage.lines.Add(newLine);
                }
                
            }
            else if(currentPage!=null)
            {
                newContainer.pages.Add(currentPage.id, currentPage);
                currentPage = null;
            }
        }

        if (currentPage != null)
        {
            newContainer.pages.Add(currentPage.id, currentPage);
            currentPage = null;
        }

        allContainers.Add(newContainer.id, newContainer);
        return newContainer;
    }
}