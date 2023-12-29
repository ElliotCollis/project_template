using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.AddressableAssets;

namespace HowlingMan
{
    public class UIManager : MonoBehaviour
    {
        public CanvasGroup currentCanvas = null;

        public CanvasGroup currentHeader = null;

        public CanvasGroup currentFooter = null;

        [Space]

        public EventSystem eventSystem;

        public CanvasGroup fadeLayer;

        public CanvasGroup loadingLayer;

        Queue<string> menuTree = new Queue<string>();

        AddressableAssetLoader assetLoader;

        //AsyncOperationHandle<GameObject> loadedMenu;

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

        public void LoadMenu(string menuName)
        {
            if (menuName == "MainMenuPrefab") menuTree.Clear();

            LoadMenuAsync(menuName, 0);
        }

        public void LoadHeader(string headerName)
        {
            LoadMenuAsync(headerName, 1);
        }

        public void LoadFooter(string footerName)
        {
            LoadMenuAsync(footerName, 2);
        }

        async void LoadMenuAsync(string menuName, int menuLevel = 0)
        {
            Debug.Log(menuName);

            if (string.IsNullOrEmpty(menuName) || menuName == "")
                return;

            GameObject loadedMenu = await assetLoader.LoadAssetAndInstantiateAsync<GameObject>(menuName, transform);

            if (loadedMenu != null)
            {
                CanvasGroup canvasGroup = loadedMenu.GetComponent<CanvasGroup>();

                switch (menuLevel)
                {
                    case 0: // current menu
                        loadedMenu.transform.SetAsFirstSibling();
                        Debug.Log($"Add menu {menuName} to menu tree.");
                        menuTree.Enqueue(menuName);

                        if (currentCanvas != null)
                            FadeMenu(currentCanvas, 0, 0.2f, true, true, true);

                        currentCanvas = canvasGroup;
                        break;

                    case 1: // current header
                        loadedMenu.transform.SetSiblingIndex(1);

                        if (currentHeader != null)
                            FadeMenu(currentHeader, 0, 0.2f, true, true, true);

                        currentHeader = canvasGroup;
                        break;

                    case 2: // current footer
                        loadedMenu.transform.SetSiblingIndex(2);

                        if (currentFooter != null)
                            FadeMenu(currentFooter, 0, 0.2f, true, true, true);

                        currentFooter = canvasGroup;
                        break;
         
                }


                FadeMenu(canvasGroup, 1, 0.2f, false, false);

                if (loadingLayer.alpha == 1) HideLoading();
            }
            else
            {
                Debug.Log("Menu loading status: Failed");
            }
        }

        public void ClearCurrentMenus()
        {
            ClearCurrentMenu();
            ClearHeaderAndFooter();
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

        public void Back() // on android back button call this.
        {
            menuTree.Dequeue();

            if (menuTree.Count <= 1)
            {
                BackToHome();
                return;
            }

            Debug.Log($"Back : {menuTree.Count}");

            LoadMenu(menuTree.Dequeue());
        }

        public void BackToHome()
        {
            Debug.Log("Back to home");
            GameManager.instance.gameState = GameManager.GameStates.inMenu;
            LoadMenu(AssetData.MainMenuPrefab);
        }

        
    }
}
