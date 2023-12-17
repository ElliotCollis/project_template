using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace HowlingMan.UI
{
    public class DialogueBox : MonoBehaviour
    {
        public TMP_Text titleText;

        public TMP_Text dialogueText;

        public Image dialogueImage;

        public CanvasGroup canvasGroup;

        public HorizontalLayoutGroup buttonLayout;

        public List<Sprite> dialogueImages;


        public void SetTitle(string title)
        {
            titleText.text = title;
        }

        public void SetDialogue(string dialogue)
        {
            dialogueText.text = dialogue;
        }

        public void SetImage(DialogueImage imageType)
        {
            if ((int)imageType < dialogueImages.Count)
            {
                dialogueImage.sprite = dialogueImages[(int)imageType];
            }
        }

        public void SetButton(GameObject button, int buttonIndex, Action onCancel, Action onConfirm)
        {
            ButtonRef buttonRef = button.GetComponent<ButtonRef>();
            button.transform.SetParent(buttonLayout.transform);

            if (buttonIndex == 0)
            {
                buttonRef.SetButtonText("Close");
                buttonRef.button.onClick.AddListener(() =>
                {
                    CloseDialogue();
                    onCancel?.Invoke();
                });
            }
            else if (buttonIndex == 1)
            {
                buttonRef.SetButtonText("Confirm");
                buttonRef.button.onClick.AddListener(() =>
                {
                    CloseDialogue();
                    onConfirm?.Invoke();
                });
            }
        }

        public void OpenDialogue()
        {
            // Start from a smaller scale and transparent
            transform.localScale = Vector3.zero;
            canvasGroup.alpha = 0;

            // Scale up and fade in using DOTween
            transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
            canvasGroup.DOFade(1, 0.2f);
        }

        public void CloseDialogue()
        {
            // Scale down and fade out using DOTween
            transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
            canvasGroup.DOFade(0, 0.5f).OnComplete(() => Destroy(gameObject));
        }
    }
}
