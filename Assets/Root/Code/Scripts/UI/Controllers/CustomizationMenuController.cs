using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowlingMan.UI
{
    public class CustomizationMenuController : BaseMenuController
    {
        public override string[] buttons
        {
            get
            {
                return new string[]
                {
                    "NextUI",
                    "Back"
                };
            }
        }

        public override void LoadHeader() { }

        public override void LoadFooter() { }

        void NextUI()
        {
            GameManager.instance.uiManager.LoadMenu("ShopPrefab");
        }

        void Back()
        {
            Debug.Log("Back to previous menu");
            GameManager.instance.uiManager.Back();
        }
    }
}
