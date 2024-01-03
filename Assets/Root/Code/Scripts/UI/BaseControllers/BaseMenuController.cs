using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HowlingMan.UI
{
    public abstract class BaseMenuController : MonoBehaviour
    {
        public VerticalLayoutGroup layoutGroup;

        public abstract string[] buttons { get; }


        bool Initialized = false;

        AddressableAssetLoader assetLoader;


        void Awake()
        {
            if (!Initialized)
                InitializeUI();
        }

        async void InitializeUI()
        {
            layoutGroup.gameObject.SetActive(false);
            assetLoader = new AddressableAssetLoader();
            GameObject buttonPrefab = await assetLoader.LoadAssetAsync<GameObject>(AssetData.UISimpleButton);

            if (buttonPrefab != null)
            {
                foreach (var buttonToSpawn in buttons)
                {
                    string buttonName = CamelCaseToSpaced(buttonToSpawn);
                    GameObject go = Instantiate(buttonPrefab, layoutGroup.transform);
                    go.transform.name = buttonName;
                    ButtonRef button = go.GetComponent<ButtonRef>();
                    button.AddButtonAction(() =>
                    {
                        Invoke(buttonToSpawn, 0f);
                    });
                    button.SetButtonText(buttonName);
                }

                assetLoader.UnloadAsset(buttonPrefab);
            }

            LoadHeader();
            LoadFooter();

            layoutGroup.gameObject.SetActive(true);
            OnInitialize();

            Initialized = true;
        }

        public virtual void OnInitialize() { }
        

        public abstract void LoadHeader();
        public abstract void LoadFooter();

        //todo move to a utilities  class.
        public static string CamelCaseToSpaced(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var newText = new StringBuilder(input.Length * 2);
            newText.Append(input[0]);

            for (int i = 1; i < input.Length; i++)
            {
                if (char.IsUpper(input[i]))
                    newText.Append(' ');
                newText.Append(input[i]);
            }

            return newText.ToString();
        }
    }
}
