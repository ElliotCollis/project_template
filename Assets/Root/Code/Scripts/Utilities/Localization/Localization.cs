using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace HowlingMan
{
    public class Localization : MonoBehaviour
    {
        public delegate void ChangeLanguageAction();
        public static event ChangeLanguageAction OnChangeLanguage;

        public TextAsset localizationCSV;

        Dictionary<string, Dictionary<string, string>> localizations;

        string currentLanguage;
        string defaultLanguage = "English";

        void Awake()
        {
            LoadLocalizations();
        }

         void Update()
        {
            if(Input.GetKeyDown(KeyCode.Z))
            {
                SetLanguage("English");
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                SetLanguage("Japanese");
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                SetLanguage("Maori");
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log(GetLocalizedText("hello"));
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Debug.Log(GetLocalizedText("good_morning"));
            }
        }

        private void LoadLocalizations()
        {
            localizations = new Dictionary<string, Dictionary<string, string>>();

            string[] lines = localizationCSV.text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            string[] languages = lines[0].Split(',');

            for (int i = 1; i < lines.Length; i++)
            {
                string[] contents = lines[i].Split(',');
                string key = contents[0];

                for (int j = 1; j < languages.Length; j++)
                {
                    if (!localizations.ContainsKey(languages[j]))
                        localizations[languages[j]] = new Dictionary<string, string>();

                    if (j < contents.Length)
                    {
                        localizations[languages[j]][key] = contents[j];
                    }
                }
            }

            SetLanguage(defaultLanguage);
        }

        public void SetLanguage(string language)
        {
            if (localizations.ContainsKey(language))
            {
                currentLanguage = language;
                Debug.Log("Localization language changed to " + currentLanguage);

                OnChangeLanguage?.Invoke();
            }
            else
            {
                Debug.LogError("Language not found: " + language);
            }
        }

        public string GetLocalizedText(string key)
        {
            if (!string.IsNullOrEmpty(currentLanguage) && localizations[currentLanguage].ContainsKey(key))
            {
                return localizations[currentLanguage][key];
            }
            return "Localization not found for key: " + key;
        }
    }
}
