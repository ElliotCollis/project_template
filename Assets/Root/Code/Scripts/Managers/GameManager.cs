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
        MenuInput menuInput;
        public GameStates gameState;

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
            if (!menuInput.actions.pauseGame.WasPressed)
                return;

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

        public void PauseGame()
        {
            if (Time.time == 0)
                Resume();
            else
                Pause();
        }

        void Pause ()
        { 
            // pause menu ui;
            uiManager.LoadMenu(AssetData.PauseMenuPrefab);
            uiManager.StoreHeaderAndFooter();
            Time.timeScale = 0;
        }


        void Resume()
        {
            uiManager.ClearCurrentMenu();
            uiManager.RestoreHeaderAndFooter();
            Time.timeScale = 1;
        }

    }
}
