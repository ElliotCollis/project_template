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
                    "Start_Game",
                    "Options",
                    "Customisation",
                    "Shop"
                #if UNITY_EDITOR || UNITY_STANDALONE || UNITY_STANDALONE_OSX
                    , "Exit"
                # endif
                };
            }
        }

        public async override void LoadHeader()
        {
            await GameManager.instance.uiManager.LoadHeader("MainMenuHeaderPrefab");
        }

        public async override void LoadFooter()
        {
            await GameManager.instance.uiManager.LoadFooter("MainMenuFooterPrefab");
        }


        void Start_Game()
        {
            Debug.Log("start the game");
            GameManager.instance.gameState = GameManager.GameStates.inGame;
            GameManager.instance.levelManager.LoadLevel("Gameplay"); 
        }

        async void Options()
        {
            Debug.Log("load options menu");
            await GameManager.instance.uiManager.LoadMenu("OptionsPrefab");
        }

        async void Customisation()
        {
            Debug.Log("load customization menu");
            await GameManager.instance.uiManager.LoadMenu("CustomizationPrefab");
        }

        async void Shop()
        {
            Debug.Log("load the shop");
            await GameManager.instance.uiManager.LoadMenu("ShopPrefab");
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
