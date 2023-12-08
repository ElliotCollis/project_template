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
                    "Back",
                    "NextUI"
                };
            }
        }

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
