using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace HowlingMan.UI
{
    public class MainMenuHeaderController : MonoBehaviour
    {
        public TMP_Text coins;
        public TMP_Text gems;
        public TMP_Text tokens;

        public Image coinImage;
        public Image gemImage;
        public Image tokenImage;

        void Awake()
        {
            coins.text = $"{LiveData.coins}";
            gems.text = $"{LiveData.gems}";
            tokens.text = $"{LiveData.tokens}";
        }
    }
}
