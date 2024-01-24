using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace HowlingMan
{
    public class UIManager : MonoBehaviour
    {
        public Canvas headerCanvas;

        public Canvas footerCanvas;

        [Space]

        public CanvasGroup currentCanvas = null;

        public CanvasGroup currentHeader = null;

        public CanvasGroup currentFooter = null;

        [Space]

        public EventSystem eventSystem;

        public CanvasGroup fadeLayer;

        public CanvasGroup loadingLayer;

        public enum MenuType
        {
            Main,
            Header,
            Footer
        }

        Queue<string> menuTree = new Queue<string>();

        AddressableAssetLoader assetLoader;

        Dictionary<string, CanvasGroup> loadedMenus = new Dictionary<string, CanvasGroup>();


        private void Start()
        {
            assetLoader = new AddressableAssetLoader();
        }

        public void FadeInFromBlack() => FadeMenu(fadeLayer, 0f, GlobalData.fadeTimes, true, true);

        public void FadeOutToBlack() => FadeMenu(fadeLayer, 1f, GlobalData.fadeTimes);

        public void ShowLoading() => FadeMenu(loadingLayer, 1f, GlobalData.fadeTimes);

        public void HideLoading() => FadeMenu(loadingLayer, 0f, GlobalData.fadeTimes, true, true);

        void FadeMenu(CanvasGroup canvasGroup, float endValue, float duration, bool startFullAlpha = false, bool hideOnComplete = false, bool destroyOnComplete = false)
        {
            canvasGroup.gameObject.SetActive(true);
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

            if (startFullAlpha) canvasGroup.alpha = 1;

            canvasGroup.DOFade(endValue, duration).OnComplete(() =>
            {
                if (destroyOnComplete)
                {
                    Destroy(canvasGroup.gameObject);
                    return;
                }

                if (!hideOnComplete)
                    return;

                canvasGroup.blocksRaycasts = false;
                canvasGroup.interactable = false;
                canvasGroup.gameObject.SetActive(false);

            }).SetUpdate(true);
        }


        public async Task PreloadMenus (string[] menuNames)
        {
            foreach (string menu in menuNames)
            {
                if (loadedMenus.ContainsKey(menu))
                {
                    // could make sure to turn it off here maybe?
                    continue;
                }

                CanvasGroup canvasGroup = await LoadMenuAsync(menu, transform, MenuType.Main, true);
                loadedMenus.Add(menu, canvasGroup);
            }
        }

        public async Task LoadMenu(string menuName)
        {
            if (menuName == "MainMenuPrefab") menuTree.Clear();

            if(loadedMenus.TryGetValue(menuName, out CanvasGroup canvasGroup))
            {
                if (currentCanvas != null)
                    FadeMenu(currentCanvas, 0, 0.2f, true, true, !loadedMenus.ContainsKey(menuName));

                currentCanvas = canvasGroup;

                Debug.Log($"Add menu {menuName} to menu tree.");
                menuTree.Enqueue(menuName);

                FadeMenu(canvasGroup, 1, 0.2f);

                return;
            }

            await LoadMenuAsync(menuName, transform, MenuType.Main);
        }

        public async Task LoadHeader(string headerName)
        {
            await LoadMenuAsync(headerName, headerCanvas.transform, MenuType.Header);
        }

        public async Task LoadFooter(string footerName)
        {
            await LoadMenuAsync(footerName, footerCanvas.transform, MenuType.Footer);
        }

        public async Task<CanvasGroup> LoadMenuAsync(string menuName, Transform canvas, MenuType menuType, bool preload = false)
        {
            Debug.Log(menuName);

            if (string.IsNullOrEmpty(menuName))
            {
                Debug.LogWarning("Menu name is null or empty");
                return null;
            }

            GameObject loadedMenu = await assetLoader.LoadAssetAndInstantiateAsync<GameObject>(menuName, canvas);

            if (loadedMenu != null)
            {
                loadedMenu.transform.SetAsFirstSibling();
                CanvasGroup canvasGroup = loadedMenu.GetComponent<CanvasGroup>();

                if(preload)
                {
                    canvasGroup.gameObject.SetActive(false);
                    return canvasGroup;
                }

                ProcessMenu(menuType, canvasGroup);

                if (menuType == MenuType.Main)
                {
                    Debug.Log($"Add menu {menuName} to menu tree.");
                    menuTree.Enqueue(menuName);
                }

                FadeMenu(canvasGroup, 1, 0.2f);

                return canvasGroup;
            }
            
            else
            {
                Debug.LogWarning("Menu loading status: Failed");
                return null;
            }
        }

        private void ProcessMenu(MenuType menuType, CanvasGroup canvasGroup)
        {
            switch (menuType)
            {
                case MenuType.Main:
                    if (currentCanvas != null)
                        FadeMenu(currentCanvas, 0, 0.2f, true, true, true);

                    currentCanvas = canvasGroup;
                    break;
                case MenuType.Header:
                    if (currentHeader != null)
                        FadeMenu(currentHeader, 0, 0.2f, true, true, true);

                    currentHeader = canvasGroup;
                    break;
                case MenuType.Footer:
                    if (currentFooter != null)
                        FadeMenu(currentFooter, 0, 0.2f, true, true, true);

                    currentFooter = canvasGroup;
                    break;
            }
        }

        public void ClearCurrentMenus()
        {
            ClearCurrentMenu();
            ClearHeaderAndFooter();
            ClearPreloadedMenus();
        }

        public void ClearCurrentMenu()
        {
            if (currentCanvas != null)
                FadeMenu(currentCanvas, 0, 0.2f, true, true, true);
            currentCanvas = null;
        }

        public void ClearHeaderAndFooter()
        {
            if (currentHeader != null)
                FadeMenu(currentHeader, 0, 0.2f, true, true, true);
            currentHeader = null;

            if (currentFooter != null)
                FadeMenu(currentFooter, 0, 0.2f, true, true, true);
            currentFooter = null;
        }

        public void StoreHeaderAndFooter()
        {
            if (currentHeader != null)
                currentCanvas.gameObject.SetActive(false);
            if (currentFooter != null)
                currentFooter.gameObject.SetActive(false);
        }

        public void RestoreHeaderAndFooter()
        {
            if (currentHeader != null)
                currentCanvas.gameObject.SetActive(true);
            if (currentFooter != null)
                currentFooter.gameObject.SetActive(true);
        }

        public void ClearPreloadedMenus ()
        {
            // todo delte old menus before moving on!
            // maybe find a way to keep options around?? unless in game and in menu are slighyl differen

            loadedMenus.Clear();
        }

        async public void Back() // on android back button call this.
        {
            menuTree.Dequeue();

            if (menuTree.Count <= 1)
            {
                BackToHome();
                return;
            }

            Debug.Log($"Back : {menuTree.Count}");

            await LoadMenu(menuTree.Dequeue());
        }

        async public void BackToHome()
        {
            Debug.Log("Back to home");
            GameManager.instance.gameState = GameManager.GameStates.inMenu;
            await LoadMenu(AssetData.MainMenuPrefab);
        }
    }
}