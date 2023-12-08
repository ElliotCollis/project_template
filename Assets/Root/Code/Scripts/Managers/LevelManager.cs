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

        public bool paused = false;
        bool loading = false;

        public int[] LevelSeeds;

        public string mainMenuPath = "MainMenuPrefab";

        public void StartGameLoad()
        {
            if (SceneManager.sceneCount == 1) // this checks if we have other scenes open for testing. If it's 1 we only have the settings open, so act like a preload.
            {
                LoadLevel(mainMenu, mainMenuPath);
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
            GameManager.instance.uiManager.FadeInFromBlack();
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

            if (menuToLoad != null)
                GameManager.instance.uiManager.LoadMenu(menuToLoad);

            
            asyncOperation.allowSceneActivation = true;
            yield return new WaitForEndOfFrame();

            //GameManager.instance.uiManager.HideLoading();
            currentLevel = targetLevel;
            targetLevel = "";
            loading = false;

            // unity scripting allow scene activation if I want a progress bar.
        }

        public void PauseGame()
        {
            paused = true;
            //GameManager.instance.uiManager.LoadPauseCanvas();
            Time.timeScale = 0;
        }

        public void ResumeGame()
        {
            //GameManager.instance.uiManager.HideAllCanvases();
            Time.timeScale = 1;
            paused = false;
        }
    }
}
