using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    public UIManager uiManager;
    public void Back () // set the options button as the first selected.
    {
        switch (uiManager.OptionsRetunMenu)
        {
            case "MainMenu":
                uiManager.LoadMainMenuCanvas();
                break;
            case "HubWorldMenu":
                uiManager.LoadHudWorldCanvas();
                break;
            case "PauseMenu":
                uiManager.LoadPauseCanvas();
                break;
            default:
                break;
        }           
    }

    // add all the options options here.
}
