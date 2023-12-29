using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowlingMan.UI
{
    public class MainMenuFooterController : BaseFooterController
    {
        PopUpDialogueController PopUpDialogueController;

        private void Awake()
        {
            PopUpDialogueController = new PopUpDialogueController();
        }

        public void ButtonClick () // pop up dialogue test.
        {
            PopUpDialogueController.ShowDialogueAsync("Hello World", "This is a test dialogue, we also need to make a big version I think, with run on scrolling text options. This is a test dialogue, we also need to make a big version I think, with run on scrolling text options. This is a test dialogue, we also need to make a big version I think, with run on scrolling text options.", DialogueImage.notice);
        }

        public void ButtonClick2() // pop up dialogue test.
        {
            PopUpDialogueController.ShowDialogueAsync("Hello World", "This is another test dialogue.", DialogueImage.notice, ()=> Debug.Log("On Cancel clicked"), () => Debug.Log("On Confirm clicked"));
        }
    }
}
