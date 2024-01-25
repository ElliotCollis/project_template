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
        AddressableAssetLoader AssetLoader;

        GameObject uiButtonRef;
        GameObject uiCheckBoxRef;
        GameObject uiDropdownRef;
        GameObject uiInputFieldRef;
        GameObject uiSliderRef;

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

        public async override void OnInitialize()
        {
            tabController = new TabController();
            AssetLoader = new AddressableAssetLoader();

            await SpawnOptionTabs();

            base.OnInitialize();
        }

        async Task SpawnOptionTabs()
        {
            string[] tabNames = { "General", "Audio", "Display", "Controls" }; // Assuming "Controls" is included

            // Map each tab name to a corresponding setup method
            var optionsSetup = new Dictionary<string, Func<Transform, Task>>
            {
                ["General"] = GeneralOptions,
                ["Audio"] = AudioOptions,
                ["Display"] = DisplayOptions,
                ["Controls"] = ControlOptions
            };

            string[] tabPanels = { AssetData.TabPanelContainer, AssetData.TabPanelContainer, AssetData.TabPanelContainer, AssetData.TabPanelContainer };
            await tabController.InitializeAsync(tapPanel, tabNames, tabPanels);

            // Initiate loading of all assets concurrently
            Task<GameObject> loadButtonTask = AssetLoader.LoadAssetAsync<GameObject>(AssetData.UIOptionsButton);
            Task<GameObject> loadCheckBoxTask = AssetLoader.LoadAssetAsync<GameObject>(AssetData.UIOptionsCheckBox);
            Task<GameObject> loadDropdownTask = AssetLoader.LoadAssetAsync<GameObject>(AssetData.UIOptionsDropdown);
            Task<GameObject> loadInputFieldTask = AssetLoader.LoadAssetAsync<GameObject>(AssetData.UIOptionsInputField);
            Task<GameObject> loadSliderTask = AssetLoader.LoadAssetAsync<GameObject>(AssetData.UIOptionsSlider);

            // Await the completion of all load tasks
            await Task.WhenAll(loadButtonTask, loadCheckBoxTask, loadDropdownTask, loadInputFieldTask, loadSliderTask);

            // Retrieve the loaded assets
            uiButtonRef = loadButtonTask.Result;
            uiCheckBoxRef = loadCheckBoxTask.Result;
            uiDropdownRef = loadDropdownTask.Result;
            uiInputFieldRef = loadInputFieldTask.Result;
            uiSliderRef = loadSliderTask.Result;

            List<Task> optionSetupTasks = new List<Task>();

            for (int i = 0; i < tabController.tabPanels.Count; i++)
            {
                string tabName = tabNames[i];
                PanalRef panalRef = tabController.tabPanels[i].GetComponent<PanalRef>();

                if (optionsSetup.TryGetValue(tabName, out var setupAction))
                {
                    optionSetupTasks.Add(setupAction(panalRef.content));
                }
            }

            await Task.WhenAll(optionSetupTasks);

            AssetLoader.UnloadAsset(uiButtonRef);
            AssetLoader.UnloadAsset(uiCheckBoxRef);
            AssetLoader.UnloadAsset(uiDropdownRef);
            AssetLoader.UnloadAsset(uiInputFieldRef);
            AssetLoader.UnloadAsset(uiSliderRef);
        }

        public override void LoadHeader() { }

        public override void LoadFooter() { }

        void Back() 
        {
            Debug.Log("Back to previous menu");
            GameManager.instance.uiManager.Back();
        }

        Task GeneralOptions(Transform panel)
        {
            AddDropdown(panel, "Resolution", (x) => Debug.Log("Resolution: " + x), new List<string>() { "empty" });
            AddCheckbox(panel, "Fullscreen", (x) => Debug.Log("Fullscreen: " + x), true);
            AddCheckbox(panel, "V-Sync", (x) => Debug.Log("V-Sync: " + x), true);
            AddDropdown(panel, "Quality", (x) => Debug.Log("Quality: " + x), new List<string>() { "empty" });
            AddDropdown(panel, "Language", (x) => Debug.Log("Language: " + x), new List<string>() { "empty" });
            AddButton(panel, "Reset All", () => Debug.Log("Reset All Clicked"));

            return Task.CompletedTask;
        }

        Task AudioOptions(Transform panel)
        {
            AddSlider(panel, "Master Volume", (x) => GameManager.instance.playerSettings.MasterVolume = x, GameManager.instance.playerSettings.MasterVolume);
            AddSlider(panel, "Music Volume", (x) => GameManager.instance.playerSettings.MusicVolume = x, GameManager.instance.playerSettings.MusicVolume);
            AddSlider(panel, "SFX Volume", (x) => GameManager.instance.playerSettings.SfxVolume = x, GameManager.instance.playerSettings.SfxVolume);
            AddCheckbox(panel, "Mute All", (x) => GameManager.instance.playerSettings.MuteAllAduio  =  x, GameManager.instance.playerSettings.MuteAllAduio);
            // ... other audio options ...
            return Task.CompletedTask;
        }

        Task DisplayOptions(Transform panel)
        {
            AddCheckbox(panel, "Subtitles", (x) => Debug.Log("Subtitles: " + x),  false);
            AddCheckbox(panel, "Colorblind Mode", (x) => Debug.Log("Colorblind Mode: " + x), false);
            AddSlider(panel, "UI Scaling", (x) => Debug.Log("UI Scaling: " + x), 0.5f);
            // ... other display options ...
            /*
            HUD Customization (Show/Hide various HUD elements)
            Accessibility options like colorblind mode, subtitles size, etc.
            Theme or Color Scheme Options
            Performance metrics display options
             */
            return Task.CompletedTask;
        }

        Task ControlOptions(Transform panel)
        {
            AddCheckbox(panel, "Invert Y-Axis", (x) => Debug.Log("Invert Y-Axis: " + x), false);
            
            /*
            Rebindable keys/buttons for different actions (if applicable)
            Controller sensitivity sliders (for joystick sensitivity, if applicable)
            Invert Y-axis Toggle (for camera controls)
             */
            return Task.CompletedTask;
        }

        // Utility methods to add UI elements
        void AddDropdown(Transform parent, string label, Action<int> callback, List<string> defaultValues)
        {
            var obj = Instantiate(uiDropdownRef, parent);
            var comp = obj.GetComponent<DropdownRef>();
            comp.dropdownText.text = label;
            comp.dropdown.AddOptions(defaultValues);
            comp.dropdown.onValueChanged.AddListener(callback.Invoke);
        }

        void AddCheckbox(Transform parent, string label, Action<bool> callback, bool defaultValue)
        {
            var obj = Instantiate(uiCheckBoxRef, parent);
            var comp = obj.GetComponent<CheckBoxRef>();
            comp.toggleText.text = label;
            comp.toggle.isOn = defaultValue;
            comp.toggle.onValueChanged.AddListener(callback.Invoke);
        }

        void AddButton(Transform parent, string label, Action callback)
        {
            var obj = Instantiate(uiButtonRef, parent);
            var comp = obj.GetComponent<ButtonRef>();
            comp.buttonText.text = label;
            comp.button.onClick.AddListener(callback.Invoke);
        }

        void AddSlider(Transform parent, string label, Action<float> callback, float defaultValue)
        {
            var obj = Instantiate(uiSliderRef, parent);
            var comp = obj.GetComponent<SliderRef>();
            comp.sliderText.text = label;
            comp.slider.value = defaultValue;
            comp.slider.onValueChanged.AddListener(callback.Invoke);
        }

        void AddInputField(Transform parent, string label, Action<string> callback, string defaultValue)
        {
            var obj = Instantiate(uiInputFieldRef, parent);
            var comp = obj.GetComponent<InputFieldRef>();
            comp.inputField.text = defaultValue;
            comp.inputField.onValueChanged.AddListener(callback.Invoke);
        }
    }
}
