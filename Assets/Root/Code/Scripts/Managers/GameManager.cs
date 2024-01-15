using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowlingMan
{
    public class GameManager : MonoBehaviour
    {
        public enum GameStates
        {
            inMenu,
            inGame,
            inCutscene
        }

        public static GameManager instance = null;
        public LevelManager levelManager;
        public UIManager uiManager;
        public Localization localization;

        MenuInput menuInput;
        public GameStates gameState;
        public bool gamePaused = false;

        void Awake()
        {
            if (instance == null) instance = this;
            else if (instance != null) Destroy(this);

            menuInput = new MenuInput();
            gameState = GameStates.inMenu;
            levelManager.StartGameLoad();
        }

        void Update()
        {
            if (menuInput.actions.pauseGame.WasPressed)
            {
                switch (gameState)
                {
                    case GameStates.inMenu:
                        uiManager.Back();
                        break;
                    case GameStates.inGame:
                        PauseGame();
                        break;
                    case GameStates.inCutscene:
                        // skip or end.
                        break;
                    default:
                        break;
                }
            }
        }

        public void PauseGame()
        {
            if (gamePaused)
                Resume();
            else
                Pause();
        }

        void Pause ()
        {
            gamePaused = true;
            uiManager.LoadMenu(AssetData.PauseMenuPrefab);
            uiManager.StoreHeaderAndFooter();
            Time.timeScale = 0;
        }


        void Resume()
        {
            gamePaused = false;
            uiManager.ClearCurrentMenu();
            uiManager.RestoreHeaderAndFooter();
            Time.timeScale = 1;
        }

    }
}
