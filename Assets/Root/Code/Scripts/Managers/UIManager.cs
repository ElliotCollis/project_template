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

        public void FadeInFromBlack() => FadeMenu(fadeLayer, 0f, 0.2f, true, true);

        public void FadeOutToBlack() => FadeMenu(fadeLayer, 1f, 0.2f);

        public void ShowLoading() => FadeMenu(loadingLayer, 1f, 0.4f);       

        public void HideLoading() => FadeMenu(loadingLayer, 0f, 0.4f, true, true);

        void FadeMenu(CanvasGroup canvasGroup, float endValue, float duration, bool startFullAlpha = false, bool hideOnComplete = false, bool destroyOnComplete =  false)
        {
            canvasGroup.gameObject.SetActive(true);
            canvasGroup.interactable = true; // might need to move this to after load.
            canvasGroup.blocksRaycasts = true;
            if(startFullAlpha) canvasGroup.alpha = 1;

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

            });
        }

        public void LoadMenu (string menuName)
        {
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
            GameObject loadedMenu = await assetLoader.LoadAssetAndInstantiateAsync<GameObject>(menuName, transform);

            if (loadedMenu != null)
            {
                CanvasGroup canvasGroup = loadedMenu.GetComponent<CanvasGroup>();

                switch (menuLevel)
                {
                    case 0: // current menu
                        loadedMenu.transform.SetAsFirstSibling();
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


                FadeMenu(canvasGroup, 1, 0.2f,  false, false);

                if (loadingLayer.alpha == 1) HideLoading();
            }else
            {
                Debug.Log("Menu loading status: Failed");
            }
        }

        public void ClearCurrentMenus  ()
        {
            if (currentCanvas != null)
                FadeMenu(currentCanvas, 0, 0.2f, true, true, true);
            currentCanvas = null;

            if (currentHeader != null)
                FadeMenu(currentHeader, 0, 0.2f, true, true, true);
            currentHeader =  null;

            if (currentFooter != null)
                FadeMenu(currentFooter, 0, 0.2f, true, true, true);
            currentFooter = null;
        }

        public void Back() // on android back button call this.
        {
            menuTree.Dequeue();

            if (menuTree.Count == 1)
            {
                BackToHome();
                return;
            }

            LoadMenu(menuTree.Dequeue());
        }

        public void BackToHome()
        {
            menuTree.Clear();
            LoadMenu("MainMenuPrefab");
        }
    }
}
