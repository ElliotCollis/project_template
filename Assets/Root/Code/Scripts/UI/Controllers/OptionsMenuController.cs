using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowlingMan.UI
{
    public class OptionsMenuController : BaseMenuController
    {
        public Transform tapPanel;
        TabController tabController;

        public override string[] buttons
        {
            get
            {
                return new string[]
                {
                    "Back"
                };
            }
        }

        public override void OnInitialize()
        {
            tabController = new TabController();
            SpawnOptionTabs();
        }

        async void SpawnOptionTabs()
        {
            string[] tabNames = new string[] { "General", "Audio", "Display", "Controls" }; // I guess controls is ommited from mobile? 
            string[] tabPanels = new string[] { AssetData.TabPanelContainer, AssetData.TabPanelContainer, AssetData.TabPanelContainer };
           
            await tabController.InitializeAsync(tapPanel, tabNames, tabPanels);

            for (int i = 0; i < tabController.tabPanels.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        GeneralOptions(tabController.tabPanels[i].transform);
                        break;
                    case 1:
                        AudioOptions(tabController.tabPanels[i].transform);
                        break;
                    case 2:
                        DisplayOptions(tabController.tabPanels[i].transform);
                        break;
                    case 3:
                        ControlOptions(tabController.tabPanels[i].transform);
                        break;
                }
            }
        }

        public override void LoadHeader() { }

        public override void LoadFooter() { }

        void Back() 
        {
            Debug.Log("Back to previous menu");
            GameManager.instance.uiManager.Back();
        }

        void GeneralOptions (Transform panel)
        {

        }

        void AudioOptions(Transform panel)
        {

        }

        void DisplayOptions(Transform panel)
        {

        }

        void ControlOptions(Transform panel)
        {

        }
    }
}
