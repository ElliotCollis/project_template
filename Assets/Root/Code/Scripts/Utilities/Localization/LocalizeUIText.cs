using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace HowlingMan
{
    public class LocalizeUIText : MonoBehaviour
    {
        public string text = null;
        TMP_Text textField;

        void OnEnable()
        {
            textField = GetComponent<TMP_Text>();
            if (string.IsNullOrEmpty(text))
            {
                text = textField.text;
            }

           // UpdateText();

            Localization.OnChangeLanguage += UpdateText;
        }

        void OnDisable()
        {
            Localization.OnChangeLanguage -= UpdateText;

        }

        public void UpdateText()
        {
            textField.text = GameManager.instance.localization.GetLocalizedText(text);
        }
    }
}
