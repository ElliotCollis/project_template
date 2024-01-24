using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowlingMan
{
    public class GameplaySceneController : MonoBehaviour
    {
        private void Awake()
        {
            PreloadMenus(); 
        }

        private async void PreloadMenus()
        {
            // List all menus to be preloaded
            string[] menusToPreload = new string[]
            {
                AssetData.PauseMenuPrefab,
                AssetData.OptionsPrefab
            };

            // pre load menus
            await GameManager.instance.uiManager.PreloadMenus(menusToPreload);

            GameManager.instance.uiManager.HideLoading();
        }
    }
}
