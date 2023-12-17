using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace HowlingMan.UI
{
    public class ButtonRef : MonoBehaviour
    {
        public Button button;

        public TMP_Text buttonText;


        public void AddButtonAction (Action buttonAction)
        {
            button.onClick.AddListener(() =>
            {
                buttonAction?.Invoke();
                button.interactable = false;
            });
        }

        public void SetButtonText (string txt)
        {
            buttonText.text = txt;
        }
    }
}
