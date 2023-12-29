using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowlingMan.UI
{
    public class PauseMenuController : BaseMenuController
    {
        public override string[] buttons
        {
            get
            {
                return new string[]
                {
                    "Resume",
                    "Options",
                    "Back"
                };
            }
        }
        public override void LoadHeader() { }

        public override void LoadFooter() { }

        void Resume()
        {
            Debug.Log("Back to Main Menu");
            GameManager.instance.uiManager.BackToHome();
        }

        void Options ()
        {
            GameManager.instance.uiManager.LoadMenu(AssetData.OptionsPrefab);
        }

        void Back()
        {
            Debug.Log("Back to Main Menu");
            GameManager.instance.uiManager.BackToHome();
        }
    }
}
