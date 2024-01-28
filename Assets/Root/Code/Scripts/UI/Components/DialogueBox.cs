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
                    GameManager.instance.audioManager.PlaySFX(SFXList.close, SFXFolder.UI);
                    CloseDialogue();
                    onCancel?.Invoke();
                });
            }
            else if (buttonIndex == 1)
            {
                buttonRef.SetButtonText("Confirm");
                buttonRef.button.onClick.AddListener(() =>
                {
                    GameManager.instance.audioManager.PlaySFX(SFXList.confirm,  SFXFolder.UI);
                    CloseDialogue();
                    onConfirm?.Invoke();
                });
            }
        }

        public void OpenDialogue()
        {
            canvasGroup.transform.localScale = Vector3.zero;
            canvasGroup.transform.DOScale(Vector3.one, GlobalData.fadeTimes/2).SetEase(Ease.OutQuad);
            canvasGroup.DOFade(1, GlobalData.fadeTimes/2);
        }

        public void CloseDialogue()
        {
            canvasGroup.transform.DOScale(Vector3.zero, GlobalData.fadeTimes/2).SetEase(Ease.InQuad);
            canvasGroup.DOFade(0, GlobalData.fadeTimes/2).OnComplete(() => Destroy(gameObject));
        }

        // background shuts without calling the corrent oncancel action.
        // but we might not want that, we might want to force the button click.
        // so we want to choose when to set it? like if two buttons don't use it?
    }
}
