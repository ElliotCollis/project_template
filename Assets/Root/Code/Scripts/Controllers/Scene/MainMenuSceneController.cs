using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HowlingMan
{
    public class MainMenuSceneController : MonoBehaviour
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
                AssetData.MainMenuPrefab,
                AssetData.OptionsPrefab,
                AssetData.CustomizationPrefab,
                AssetData.ShopPrefab
            };

            // pre load menus
            await GameManager.instance.uiManager.PreloadMenus(menusToPreload);

            // load our main menu.
            await GameManager.instance.uiManager.LoadMenu(AssetData.MainMenuPrefab);

            GameManager.instance.audioManager.PlayBGM(MusicList.ActandContemplate);

            GameManager.instance.uiManager.HideLoading();
        }
    }
}
