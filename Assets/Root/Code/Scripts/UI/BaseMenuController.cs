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

        public ButtonRef buttonPrefab; // I could make things like this addressable..
        public string buttonPath = "UISimplePanelButton";


        public abstract string[] buttons { get; }

        bool Initialized = false;

        void Awake()
        {
            if (!Initialized)
                InitUI();
        }
        void InitUI()
        {
            foreach (var buttonToSpawn in buttons)
            {
                string buttonName = CamelCaseToSpaced(buttonToSpawn);
                ButtonRef go = Instantiate(buttonPrefab, layoutGroup.transform); // would  be good to have the addressables here.
                go.transform.name = buttonName;
                go.button.onClick.AddListener(() =>
                {
                    Invoke(buttonToSpawn, 0f);
                });
                go.buttonText.text = buttonName;
            }

            Initialized = true;
        }

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
