using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowlingMan.UI
{
    public class OptionsMenuController : BaseMenuController
    {
        public Transform tapPanel;
        TabController tabController;

        public override string[] buttons
        {
            get
            {
                return new string[]
                {
                    "NextUI",
                    "Back"
                };
            }
        }

        public override void OnInitialize()
        {
            tabController = new TabController();
            SpawnTabGroup();
        }

        async void SpawnTabGroup()
        {
            string[] tabNames = new string[] { "tab1", "tab2", "tab3" };
            string[] tabPanels = new string[] { AssetData.TabPanelContainer, AssetData.TabPanelContainer, AssetData.TabPanelContainer };
           
            await tabController.InitializeAsync(tapPanel, tabNames, tabPanels);
        }

        public override void LoadHeader() { }

        public override void LoadFooter() { }

        void NextUI()
        {
            GameManager.instance.uiManager.LoadMenu("CustomizationPrefab");
        }

        void Back() 
        {
            Debug.Log("Back to previous menu");
            GameManager.instance.uiManager.Back();
        }
    }
}
