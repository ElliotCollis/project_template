using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace HowlingMan.UI
{
    [SerializeField]
    public class TabController
    {
        public float tabButtonInactiveDelay = 0.2f;
        public PanalRef basePanel;
        public List<ButtonRef> tabButtons;
        public List<CanvasGroup> tabPanels;

        public int currentPanelIndex = -1;

        AddressableAssetLoader assetLoader;

        public TabController()
        {
            assetLoader = new AddressableAssetLoader();
        }

        public async Task InitializeAsync(Transform parent, string[] buttonNames, string[] panelAssets, int startPanelIndex = 0)
        {
            await SpawnBase(parent);
            await SpawnTabsAndPanels(buttonNames, panelAssets);
            DisplayPanel(startPanelIndex);
        }

        async Task SpawnBase(Transform parent)
        {
            GameObject basePanelPrefab = await assetLoader.LoadAssetAndInstantiateAsync<GameObject>(AssetData.LargePanel, parent);
            basePanel = basePanelPrefab.GetComponent<PanalRef>();
            basePanel.gameObject.SetActive(false);
        }

        async Task SpawnTabsAndPanels(string[] buttonNames, string[] panelAssets)
        {
            var tasks = new List<Task<GameObject>>();
            tasks.Add(assetLoader.LoadAssetAsync<GameObject>(AssetData.UISimpleButton));

            foreach (var panelName in panelAssets)
            {
                tasks.Add(assetLoader.LoadAssetAndInstantiateAsync<GameObject>(panelName, basePanel.content));
            }

            var results = await Task.WhenAll(tasks);

            SetupTabs(buttonNames, results[0]);
            SetupPanels(results, panelAssets.Length);
        }

        void SetupTabs(string[] buttonNames, GameObject buttonPrefab)
        {
            tabButtons = new List<ButtonRef>();
            for (int i = 0; i < buttonNames.Length; i++)
            {
                GameObject button = GameObject.Instantiate(buttonPrefab, basePanel.layoutGroup.transform);
                ButtonRef buttonRef = button.GetComponent<ButtonRef>();

                int index = i;  // Local variable to capture the current index
                buttonRef.AddButtonAction(() => DisplayPanel(index));
                buttonRef.SetButtonText(buttonNames[i]);
                buttonRef.inactiveDelay = tabButtonInactiveDelay;
                tabButtons.Add(buttonRef);
            }

            assetLoader.UnloadAsset(buttonPrefab);
        }

        void SetupPanels(GameObject[] panelAssets, int numberOfPanels)
        {
            tabPanels = new List<CanvasGroup>();
            for (int i = 0; i < numberOfPanels; i++)
            {
                CanvasGroup canvasGroup = panelAssets[i + 1].GetComponent<CanvasGroup>(); // +1 to skip the button prefab
                tabPanels.Add(canvasGroup);
                canvasGroup.gameObject.SetActive(false);
            }
        }

        void DisplayPanel(int index)
        {
            basePanel.gameObject.SetActive(true);

            if (currentPanelIndex >= 0)
            {
                tabPanels[currentPanelIndex].gameObject.SetActive(false);
            }

            tabPanels[index].gameObject.SetActive(true);
            currentPanelIndex = index;
        }
    }
}