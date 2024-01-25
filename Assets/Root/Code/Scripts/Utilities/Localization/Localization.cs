using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace HowlingMan
{
    public class Localization : MonoBehaviour
    {
        bool initalized = false;

        public delegate void ChangeLanguageAction();
        public static event ChangeLanguageAction OnChangeLanguage;

        public TextAsset localizationCSV;

        Dictionary<string, Dictionary<string, string>> localizations;

        public enum SupportedLanguages
        {
            English,
            Māori,
            Japanese,
            Korean,
            Chinese,
            French,
            Spanish,
            Portuguese,
            German,
            Arabic,
            Russian
        }

        [Space(20)]

        [SerializeField] TMP_FontAsset romanicFont;
        [SerializeField] TMP_FontAsset japaneseFont;
        [SerializeField] TMP_FontAsset chineseFont;
        [SerializeField] TMP_FontAsset koreanFont;
        [SerializeField] TMP_FontAsset arabicFont;

        public SupportedLanguages currentLanguage;

        public void Initalize()
        {
            if (!initalized)
                LoadLocalizations();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                currentLanguage = (SupportedLanguages)(((int)currentLanguage + 1) % System.Enum.GetValues(typeof(SupportedLanguages)).Length);

                OnChangeLanguage?.Invoke();
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

            initalized = true;
        }

        public void SetLanguage(SupportedLanguages language)
        {
            if (!initalized)
                LoadLocalizations();

            if (localizations.ContainsKey(language.ToString()))
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
            if (!initalized)
                LoadLocalizations();

            if (!string.IsNullOrEmpty(currentLanguage.ToString()) && localizations[currentLanguage.ToString()].ContainsKey(key))
            {
                return localizations[currentLanguage.ToString()][key];
            }
            
            return key;
        }

        public TMP_FontAsset GetCurrentFont()
        {
            if (currentLanguage == SupportedLanguages.Māori
                || currentLanguage == SupportedLanguages.French
                || currentLanguage == SupportedLanguages.Spanish
                || currentLanguage == SupportedLanguages.Portuguese
                || currentLanguage == SupportedLanguages.German
                || currentLanguage == SupportedLanguages.Russian)
            {
                return romanicFont;
            }

            switch (currentLanguage)
            {
                case SupportedLanguages.Japanese:
                    return japaneseFont;
                case SupportedLanguages.Korean:
                    return koreanFont;
                case SupportedLanguages.Chinese:
                    return chineseFont;
                case SupportedLanguages.Arabic:
                    return arabicFont;
            }

            return romanicFont;
        }
    }
}
