using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HowlingMan.UI
{
    public class MainMenuController : BaseMenuController
    {
        public override string[] buttons
        {
            get
            {
                return new string[]
                {
                    "StartGame",
                    "Options",
                    "Customization",
                    "Shop"
                #if UNITY_EDITOR || UNITY_STANDALONE || UNITY_STANDALONE_OSX
                    , "Exit"
                # endif
                };
            }
        }

        public override void LoadHeader()
        {
            GameManager.instance.uiManager.LoadHeader("MainMenuFooterPrefab");
        }

        public override void LoadFooter()
        {
            GameManager.instance.uiManager.LoadFooter("MainMenuFooterPrefab");
        }


        void StartGame()
        {
            Debug.Log("start the game");
            GameManager.instance.levelManager.LoadLevel("Gameplay"); 
        }

        void Options()
        {
            Debug.Log("load options menu");
            GameManager.instance.uiManager.LoadMenu("OptionsPrefab");
        }

        void Customization()
        {
            Debug.Log("load customization menu");
            GameManager.instance.uiManager.LoadMenu("CustomizationPrefab");
        }

        void Shop()
        {
            Debug.Log("load the shop");
            GameManager.instance.uiManager.LoadMenu("ShopPrefab");
        }

        void Exit ()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            return;
#endif
            Application.Quit();
        }
    }
}
