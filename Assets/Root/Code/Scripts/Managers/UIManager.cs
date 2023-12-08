using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace HowlingMan
{
    public class UIManager : MonoBehaviour
    {
        public CanvasGroup currentCanvas = null;

        [Space]

        public EventSystem eventSystem;

        public CanvasGroup fadeLayer;

        public CanvasGroup loadingLayer;

        Queue<string> menuTree = new Queue<string>();

        AsyncOperationHandle<GameObject> loadedMenu;


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
            StartCoroutine("LoadMenuAsync", menuName);
        }

        public void Back () // on android back button call this.
        {
            menuTree.Dequeue();

            if (menuTree.Count == 1)
            {
                BackToHome();
                return;
            }

            LoadMenu(menuTree.Dequeue());
        }

        public void BackToHome ()
        {
            menuTree.Clear();
            LoadMenu(GameManager.instance.levelManager.mainMenuPath);
        }

        IEnumerator LoadMenuAsync(string menuName)
        {
            Debug.Log("Menu loading status: Started");

            loadedMenu = Addressables.LoadAssetAsync<GameObject>(menuName);
            yield return loadedMenu;
            Debug.Log($"Menu loading status: {loadedMenu.Status}");

            if (loadedMenu.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject go = Instantiate(loadedMenu.Result, transform);
                go.transform.SetAsFirstSibling();
                CanvasGroup canvasGroup = go.GetComponent<CanvasGroup>();

                if (currentCanvas != null)
                    FadeMenu(currentCanvas, 0, 0.2f, true, true, true);
                FadeMenu(canvasGroup, 1, 0.2f,  false, false);

                Addressables.Release(loadedMenu);

                menuTree.Enqueue(menuName);
                currentCanvas = canvasGroup;

                if (loadingLayer.alpha == 1) HideLoading();
            }else
            {
                Debug.Log("Menu loading status: Failed");
            }
        }

        void OnDestroy()
        {
            if (loadedMenu.IsValid())
            {
                Addressables.Release(loadedMenu);
            }
        }
    }
}
