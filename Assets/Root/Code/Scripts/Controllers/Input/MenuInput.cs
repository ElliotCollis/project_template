using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

namespace HowlingMan
{
    public class MenuInput
    {
        public MyMenuActions actions;

        public MenuInput()
        {
            BindControlls();
        }

        void BindControlls ()
        {
            actions = new MyMenuActions();

            //Pause
            actions.pauseGame.AddDefaultBinding(InputControlType.Start);
            actions.pauseGame.AddDefaultBinding(Key.Escape);
        }
    }
}
