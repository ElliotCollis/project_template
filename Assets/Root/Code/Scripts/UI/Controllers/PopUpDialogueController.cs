using System;
using UnityEngine;
using TMPro;

namespace HowlingMan.UI
{
    public enum DialogueImage
    {
        notice,
        alert,
        question
    }

    public class PopUpDialogueController : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text titleText;

        private AddressableAssetLoader assetLoader;

        public PopUpDialogueController()
        {
            assetLoader = new AddressableAssetLoader();
        }

        public async void ShowDialogue(string title, string dialogue, DialogueImage imageType, Action onCancel = null, Action onConfirm = null)
        {
            GameObject dialogueBox = await assetLoader.LoadAssetAndInstantiateAsync<GameObject>("DialogueBox", transform);
            DialogueBox dialogueBoxComponent = dialogueBox.GetComponent<DialogueBox>();

            dialogueBoxComponent.SetTitle(title);
            dialogueBoxComponent.SetDialogue(dialogue);
            dialogueBoxComponent.SetImage(imageType);

            int buttons = 1;

            if (onConfirm != null) buttons = 2;

            for (int i = 0; i < buttons; i++)
            {
                GameObject button = await assetLoader.LoadAssetAndInstantiateAsync<GameObject>("ButtonREF", dialogueBox.transform);
                dialogueBoxComponent.SetButton(button, i, onCancel, onConfirm);
            }

            dialogueBoxComponent.OpenDialogue();
        }
    }
}
