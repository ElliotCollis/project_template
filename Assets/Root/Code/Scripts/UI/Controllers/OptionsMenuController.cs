using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
                    "Back"
                };
            }
        }

        public override void OnInitialize()
        {
            tabController = new TabController();
            SpawnOptions();
        }

        async void SpawnOptions()
        {
            await SpawnOptionTabs();
        }

        async Task SpawnOptionTabs()
        {
            string[] tabNames = { "General", "Audio", "Display", "Controls" }; // Assuming "Controls" is included

            // Map each tab name to a corresponding setup method
            var optionsSetup = new Dictionary<string, Action<Transform>>
            {
                ["General"] = GeneralOptions,
                ["Audio"] = AudioOptions,
                ["Display"] = DisplayOptions,
                ["Controls"] = ControlOptions
            };

            string[] tabPanels = { AssetData.TabPanelContainer, AssetData.TabPanelContainer, AssetData.TabPanelContainer, AssetData.TabPanelContainer };
            await tabController.InitializeAsync(tapPanel, tabNames, tabPanels);

            for (int i = 0; i < tabController.tabPanels.Count; i++)
            {
                string tabName = tabNames[i];
                if (optionsSetup.TryGetValue(tabName, out var setupAction))
                {
                    setupAction.Invoke(tabController.tabPanels[i].transform);
                }
            }
        }

        public override void LoadHeader() { }

        public override void LoadFooter() { }

        void Back() 
        {
            Debug.Log("Back to previous menu");
            GameManager.instance.uiManager.Back();
        }

        void GeneralOptions (Transform panel)
        {

        }

        void AudioOptions(Transform panel)
        {

        }

        void DisplayOptions(Transform panel)
        {

        }

        void ControlOptions(Transform panel)
        {

        }
    }
}
