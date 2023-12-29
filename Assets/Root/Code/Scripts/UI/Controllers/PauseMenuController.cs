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
            GameManager.instance.PauseGame();
        }

        void Options ()
        {
            Debug.Log("Load options.");
            GameManager.instance.uiManager.LoadMenu(AssetData.OptionsPrefab);
        }

        void Back()
        {
            Debug.Log("Back to Main Menu");
            GameManager.instance.PauseGame();
            GameManager.instance.levelManager.LoadHome();
        }
    }
}
