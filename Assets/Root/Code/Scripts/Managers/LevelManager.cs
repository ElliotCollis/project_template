using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HowlingMan
{
    public class LevelManager : MonoBehaviour
    {
        public string mainMenu = "Main";
        public string currentLevel = "";
        public string targetLevel = "";

        public GameObject[] menus;

        bool loading = false;

        public int[] LevelSeeds;

        public void StartGameLoad()
        {
            if (SceneManager.sceneCount == 1) // this checks if we have other scenes open for testing. If it's 1 we only have the settings open, so act like a preload.
            {
                if (!loading)
                    StartCoroutine("ShowSplashScreen");
            }
            else
            {
                for (int i = 0; i < SceneManager.sceneCount; i++)
                {
                    Scene scene = SceneManager.GetSceneAt(i);
                    if (scene.buildIndex != 0)
                    {
                        currentLevel = scene.name;
                    }
                }
            }
        }

        IEnumerator ShowSplashScreen ()
        {
            loading = true;

            GameManager.instance.uiManager.FadeInFromBlack();

            yield return new WaitForSeconds(0.2f);

            GameManager.instance.uiManager.LoadMenu(AssetData.SplashScreensPrefab);

            loading = false;
        }

        public void LoadLevel(string levelName, string menuToLoad = "")
        {
            targetLevel = levelName;

            if (!loading)
                StartCoroutine("LoadScene", menuToLoad);
        }

        IEnumerator LoadScene(string menuToLoad)
        {
            loading = true;
            GameManager.instance.uiManager.ShowLoading();
            GameManager.instance.uiManager.ClearCurrentMenus();
            yield return new WaitForSeconds(1f);

            AsyncOperation asyncOperation;

            if (currentLevel != "")
            {
                asyncOperation = SceneManager.UnloadSceneAsync(currentLevel);
                while (!asyncOperation.isDone)
                {
                    yield return null;
                }
            }

            asyncOperation = SceneManager.LoadSceneAsync(targetLevel, LoadSceneMode.Additive);
            asyncOperation.allowSceneActivation = false;


            while (asyncOperation.progress < 0.9f)
            {
                yield return null;
            }

            if (menuToLoad != "")
                GameManager.instance.uiManager.LoadMenu(menuToLoad);
            else
                GameManager.instance.uiManager.HideLoading();

            asyncOperation.allowSceneActivation = true;
            yield return new WaitForEndOfFrame();

            currentLevel = targetLevel;
            targetLevel = "";
            loading = false;

            // unity scripting allow scene activation if I want a progress bar. might not work with async operations easily.
        }

        public void LoadHome()
        {
            GameManager.instance.gameState = GameManager.GameStates.inMenu;
            LoadLevel(mainMenu, AssetData.MainMenuPrefab);
        }
    }
}
