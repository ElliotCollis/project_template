using System.Text;
using System.Threading.Tasks;
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
            assetLoader = new AddressableAssetLoader();

            if (!Initialized)
                InitializeUI();
        }

        async void InitializeUI() // task?
        {
            layoutGroup.gameObject.SetActive(false);
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

            // turn into tasks
            LoadHeader(); 
            LoadFooter(); 
            OnInitialize(); // this is important because it's the other parts like the options menu

            Initialized = true;
        }

        public virtual void OnInitialize()
        { 
            layoutGroup.gameObject.SetActive(true);
        }
        

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
