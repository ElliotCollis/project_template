using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//using InControl;
using System.ComponentModel;

public class UIManager : MonoBehaviour
{
    // eventsystem reference
    public EventSystem eventSystem;
    //public InControlInputModule uiInput;

    public GameObject parent = null;

    [Space]
    // link to each canvas
    // link to first selected. keeping things simple 
    public GameObject MainMenuCanvas;
    public GameObject FirstSelectedInMainMenu;
    [Space]
    public GameObject OptionsCanvas;
    public GameObject FirstSelectedInOptions;
    [Space]
    public GameObject PauseCanvas;
    public GameObject FirstSelectedInPause;
    [Space]
    public GameObject SaveFileSelectCanvas;
    public GameObject FirstSelectedInSaveFileSelect;
    [Space]
    public GameObject HubWorldCanvas;
    public GameObject FirstSelectedInHubWorld;
    [Space]
    public GameObject WinCanvas;
    public GameObject FirstSelectedInWin;
    [Space]
    public GameObject LoseCanvas;
    public GameObject FirstSelectedInLose;
    [Space]
    public GameObject ShopCanvas;
    public GameObject FirstSelectShop;
    [Space]
    public GameObject InventoryCanvas;
    public GameObject FirstSelectInventory;
    [Space] 
    public GameObject StarFoxCanvas;
    public GameObject FirstSelectStarFox;
    [Space]
    public GameObject CharacterCanvas;
    public GameObject FirstSelectCharacter;
    [Space] 
    public GameObject NextLevelCanvas;
    public GameObject FirstSelectNextLevel;
    [Space] 
    public GameObject EndLevelCanvas;
    public GameObject FirstSelectEndLevel;
    [Space]
    public GameObject GameStatsCanvas;
    [Space]
    public GameObject HUDCanvas;
    [Space]
    public string OptionsRetunMenu = "MainMenu";
    [Space]
    // win screen
    // lose screen
    // HUD // obviously we don't want to hide the HUD? I guess we could. 

    public GameObject currentCanvas = null;

    //void Update()
    //{
    //    if (uiInput.CancelAction.WasPressed)
    //    {
    //        if (currentCanvas == OptionsCanvas)
    //        {
    //            OptionsCanvas.GetComponent<OptionsMenu>().Back();
    //            return;
    //        }

    //        if (parent == null) return;

    //        //this is going to be even cruder than a switch.
    //        if (parent == MainMenuCanvas)
    //        {
    //            LoadMainMenuCanvas();
    //        }
    //        if (parent == SaveFileSelectCanvas)
    //        {
    //           // GameManager.instance.levelManager.LoadLevel("MainMenu");
    //            //LoadSaveFileSelect();
    //        }
    //        if (parent == HubWorldCanvas)
    //        {
    //           // GameManager.instance.levelManager.LoadLevel("HubWorld");
    //            LoadHudWorldCanvas();
    //        }
    //        if (parent == PauseCanvas)
    //        {
    //            LoadPauseCanvas();
    //        }
    //        // I'll need to swap up how I handle UI to make it more streamlined and easier to manage, A simple parent child system will work.
    //        // as long as everyone takes care of their own first select etc.
    //    }
    //}

        public void LoadMainMenuCanvas ()
    {
        parent = null;
        LoadCanvas(MainMenuCanvas, FirstSelectedInMainMenu);
    }

    public void LoadOptionsCanvas()
    {
        //if (currentCanvas == MainMenuCanvas) parent = MainMenuCanvas;
        //if (currentCanvas == HubWorldCanvas) parent = HubWorldCanvas;
        //if (currentCanvas == PauseCanvas) parent = PauseCanvas;
        LoadCanvas(OptionsCanvas, FirstSelectedInOptions);
    }

    public void LoadPauseCanvas()
    {
        LoadCanvas(PauseCanvas, FirstSelectedInPause);
    }

    public void LoadSaveFileSelect()
    {
        parent = MainMenuCanvas;
        LoadCanvas(SaveFileSelectCanvas, FirstSelectedInSaveFileSelect);
    }

    public void LoadHudWorldCanvas ()
    {
        parent = SaveFileSelectCanvas;
        LoadCanvas(HubWorldCanvas, FirstSelectedInHubWorld);
    }

    public void LoadWinCanvas()
    {
        UnloadHUDCanvas();
        UnloadShopCanvas();
        UnloadInventoryCanvas();
        LoadCanvas(WinCanvas, FirstSelectedInWin);
    }

    public void LoadLoseCanvas()
    {
        UnloadHUDCanvas();
        UnloadShopCanvas();
        UnloadInventoryCanvas();
        LoadCanvas(LoseCanvas, FirstSelectedInLose);
    }

    //public void LoadNextLevelCanvas (GameEndGoal reference)
    //{
    //    //UnloadHUDCanvas();
    //    //UnloadShopCanvas();
    //    //UnloadInventoryCanvas();
    //    LoadCanvas(NextLevelCanvas, FirstSelectNextLevel);
    //    NextLevelCanvas.GetComponent<NextLevelMenu>().SetGameEndGoalReference(reference);
    //}

    public void UnloadNextLevelCanvas()
    {
        HideAllCanvases();
        //LoadHUDCanvas();
    }

    public void LoadHUDCanvas()
    {
        //UnloadHUDCanvas();
        ShowCanvas(HUDCanvas);
    }

    public void UnloadHUDCanvas ()
    {
        HideCanvas(HUDCanvas);
    }

    //public void LoadShopCanvas (ShopSetPeice reference)
    //{
    //    Time.timeScale = 0;
    //    ShowCanvas(ShopCanvas);
    //    eventSystem.SetSelectedGameObject(FirstSelectShop);
    //    ShopCanvas.GetComponent<CharacterShopMenu>().SetShopReference(reference);
    //}

    public void UnloadShopCanvas()
    {
        Time.timeScale = 1;
       // FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UIOpen", transform.position);
        HideCanvas(ShopCanvas);
    }

    //public void LoadInventoryCanvas (WorkshopSetPeice reference)
    //{
    //    ShowCanvas(InventoryCanvas);
    //    eventSystem.SetSelectedGameObject(FirstSelectInventory);
    //    InventoryCanvas.GetComponent<InventoryMenu>().SetWrokshopReference(reference);
    //    // set reference to call unload?
    //}

    public void UnloadInventoryCanvas ()
    {
        HideCanvas(InventoryCanvas);
    }

    public void LoadStarFoxMap ()
    {
        parent = HubWorldCanvas;
        LoadCanvas(StarFoxCanvas, FirstSelectStarFox);
    }

    public void LoadEndLevel ()
    {
        parent = HubWorldCanvas;
        LoadCanvas(EndLevelCanvas, FirstSelectEndLevel);
    }

    public void UnloadStarFoxMap ()
    {
        HideCanvas(StarFoxCanvas);
    }

    //public void LoadCharacterCanvas (CharacterSetPeice reference, ConversationObject conversation) // add reference for the character and conversation to pass through
    //{
    //    ShowCanvas(CharacterCanvas);
    //    eventSystem.SetSelectedGameObject(FirstSelectCharacter);
    //    CharacterCanvas.GetComponent<CharacterConversationMenu>().SetCharacterSetPeice(reference, conversation);

    //}

    public void UnloadCharacterCanvas ()
    {
        HideCanvas(CharacterCanvas);
    }

    public void CheckSubMenu ()
    {
        // this checks to see if the shop or inventory are open when we close the pause menu, and set the first selected again.
        if(ShopCanvas.activeInHierarchy)
        {
            eventSystem.SetSelectedGameObject(FirstSelectShop);
        }
    }

    void LoadCanvas (GameObject canvas, GameObject firstSelected)
    {
       // FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UIOpen", transform.position);
        if (currentCanvas != null)
            HideCanvas(currentCanvas);
        ShowCanvas(canvas);
        eventSystem.SetSelectedGameObject(firstSelected);
        currentCanvas = canvas;
    }

    public void HideAllCanvases() // can do on start or something...
    {
        //FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UIOpen", transform.position);
        HideCanvas(MainMenuCanvas);
        HideCanvas(OptionsCanvas);
        HideCanvas(PauseCanvas);
        HideCanvas(SaveFileSelectCanvas);
        HideCanvas(HubWorldCanvas);
        HideCanvas(WinCanvas);
        HideCanvas(LoseCanvas);
        HideCanvas(StarFoxCanvas);
        HideCanvas(EndLevelCanvas);
        HideCanvas(NextLevelCanvas);
        currentCanvas = null;
        parent = null;
    }

    void HideCanvas(GameObject canvas)
    {
        canvas.SetActive(false);
    }

    void ShowCanvas(GameObject canvas)
    {
        canvas.SetActive(true);
    }

    void SwapCanvas(GameObject canvasToTurnOff, GameObject canvasToTurnOn) // unused
    {
        HideCanvas(canvasToTurnOff);
        ShowCanvas(canvasToTurnOn);
    }
    
}