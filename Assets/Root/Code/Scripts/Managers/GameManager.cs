using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowlingMan
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance = null;
        public LevelManager levelManager;
        public UIManager uiManager;

        void Awake()
        {
            if (instance == null) instance = this;
            else if (instance != null) Destroy(this);

            levelManager.StartGameLoad();
        }


    }
}
