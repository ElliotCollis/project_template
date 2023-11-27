using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour // similar to our main menu
{
    public UIManager uiManager;
    
    public void Resume ()
    {
        uiManager.HideAllCanvases();
        uiManager.CheckSubMenu();
      //  GameManager.instance.levelManager.ResumeGame();
    }

    public void Options ()
    {
        uiManager.OptionsRetunMenu = "PauseMenu";
        uiManager.LoadOptionsCanvas();
    }

    public void MainMenu() // this will change from main menu to back to hub world.
    {
        // show warning your about to leave.
        // player leaving animation.

        //if(GameManager.instance.levelManager.paused)
        //{
        //    GameManager.instance.levelManager.ResumeGame();
        //}
        uiManager.HideAllCanvases();
        uiManager.UnloadHUDCanvas();
        uiManager.UnloadShopCanvas();
        uiManager.UnloadInventoryCanvas();
       // GameManager.instance.levelManager.LoadLevel("HubWorld");
    }
}
