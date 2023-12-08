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

        void StartGame()
        {
            Debug.Log("start the game");
            GameManager.instance.uiManager.LoadMenu("OptionsPrefab");
        }

        void Options()
        {
            Debug.Log("load options menu");
        }

        void Customization()
        {
            Debug.Log("load customization menu");
        }

        void Shop()
        {
            Debug.Log("load the shop");
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
