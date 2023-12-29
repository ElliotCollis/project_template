using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

namespace HowlingMan
{
    public class MyMenuActions : PlayerActionSet
    {
        public PlayerAction pauseGame;

        public MyMenuActions()
        {
            pauseGame = CreatePlayerAction("Pause Game");
        }
    }
}
