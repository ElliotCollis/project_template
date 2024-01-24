using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace HowlingMan.UI
{
    public class ButtonRef : MonoBehaviour
    {
        public Button button;
        public TMP_Text buttonText;
        public float inactiveDelay = 0.4f;

        private void OnEnable()
        {
            button.interactable = true;
        }

        public void AddButtonAction(Action buttonAction)
        {
            button.onClick.AddListener(() =>
            {
                buttonAction?.Invoke();
                StartCoroutine(ReenableButtonAfterDelay(inactiveDelay));
            });
        }

        private IEnumerator ReenableButtonAfterDelay(float delay)
        {
            button.interactable = false;
            yield return new WaitForSeconds(delay);
            button.interactable = true;
        }

        public void SetButtonText(string txt)
        {
            buttonText.text = txt;
            LocalizeUIText localizeUI;
            if(buttonText.gameObject.TryGetComponent<LocalizeUIText>(out localizeUI)) 
            {
                localizeUI.text = txt.Replace(" ", "").ToLower();
                localizeUI.UpdateText();
            }
        }
    }
}
