using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//using DG.Tweening;
//using InControl;

public class LevelManager : MonoBehaviour 
{
    public Animator SceneTransitions;
    //public InControlInputModule inputModule;

    public CanvasGroup loadingScreen;
    //public string loadingLevel = "Loading";
    public string currentLevel = "";
    public string targetLevel = "";

    public bool paused = false;
    bool loading = false;

    public int[] LevelSeeds;

    public void StartGameLoad()
    {
        if (SceneManager.sceneCount == 1) // this checks if we have other scenes open for testing. If it's 1 we only have the settings open, so act like a preload.
        {
            currentLevel = "MainMenu";
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
        }else
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if(scene.buildIndex != 0)
                {
                    currentLevel = scene.name;
                }
            }
        }
        SceneTransitions.SetTrigger("FadeIn");
    }

    public void LoadLevel(string levelName) // here we're loading a scene by it's name, for everything else that needs a transition
    {
        targetLevel = levelName;

        if (!loading)
            StartCoroutine("LoadScene");
    }

    IEnumerator LoadScene()
    {
        loading = true;
        //Tween loadingTween;
        // Load in the loading screen

        loadingScreen.gameObject.SetActive(true);
       // loadingTween = loadingScreen.DOFade(1, 0.2f);
        yield return new WaitForSeconds(0.2f);
  

        SceneTransitions.SetTrigger("FadeIn");
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

        yield return new WaitForSeconds(1f);
        asyncOperation.allowSceneActivation = true; // would it feel better with a wait till end of frame after this?
        yield return new WaitForEndOfFrame();

        //loadingTween = loadingScreen.DOFade(0, 0.2f);
        yield return new WaitForSeconds(0.3f);

        loadingScreen.gameObject.SetActive(false);
        currentLevel = targetLevel;
        targetLevel = "";
        loading = false;

        // unity scripting allow scene activation if I want a progress bar.
    }

    public void PauseGame ()
    {
        paused = true;
        //GameManager.instance.uiManager.LoadPauseCanvas();
        Time.timeScale = 0; 
    }

    public void ResumeGame () 
    {
        //GameManager.instance.uiManager.HideAllCanvases();
        Time.timeScale = 1; 
        paused = false; 
    }
}
