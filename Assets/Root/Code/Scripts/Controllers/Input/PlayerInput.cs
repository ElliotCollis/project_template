using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

namespace HowlingMan
{
    public
        class PlayerInput : MonoBehaviour
    {
        public bool _inControl = false;
        public MyCharacterActions characterActions;
        public Vector2 direction = Vector2.zero;
        public float _dodgeCooldown = 1f;

        void Awake()
        {
            BindControls();
        }

        void Update()
        {
            direction.y = 0f;

            if (_inControl)
            {
                if (characterActions.up.IsPressed)
                {
                    direction.y = 1f;
                }
            }
        }

        public void BindControls() // single player controls.
        {
            characterActions = new MyCharacterActions();
            //Left
            characterActions.left.AddDefaultBinding(InputControlType.LeftStickLeft);
            characterActions.left.AddDefaultBinding(InputControlType.DPadLeft);
            characterActions.left.AddDefaultBinding(Key.LeftArrow);
            characterActions.left.AddDefaultBinding(Key.A);
            //Right
            characterActions.right.AddDefaultBinding(InputControlType.LeftStickRight);
            characterActions.right.AddDefaultBinding(InputControlType.DPadRight);
            characterActions.right.AddDefaultBinding(Key.RightArrow);
            characterActions.right.AddDefaultBinding(Key.D);
            //Fire
            characterActions.action1.AddDefaultBinding(InputControlType.Action1);
            characterActions.action1.AddDefaultBinding(Key.Space);
            characterActions.action1.AddDefaultBinding(Mouse.LeftButton);
            //Accelerate
            characterActions.r2.AddDefaultBinding(InputControlType.RightTrigger);
            characterActions.r2.AddDefaultBinding(Key.UpArrow);
            characterActions.r2.AddDefaultBinding(Key.W);
            //Boost
            characterActions.r1.AddDefaultBinding(InputControlType.RightBumper);
            characterActions.r1.AddDefaultBinding(Key.LeftShift);
            //Stop
            characterActions.l2.AddDefaultBinding(InputControlType.LeftTrigger);
            characterActions.l2.AddDefaultBinding(Key.T);
            characterActions.l2.AddDefaultBinding(Key.DownArrow);
            characterActions.l2.AddDefaultBinding(Key.S);
            //Dodge
            characterActions.action2.AddDefaultBinding(InputControlType.LeftBumper);
            //ActiveItem
            characterActions.action3.AddDefaultBinding(InputControlType.Action2);
            characterActions.action3.AddDefaultBinding(Key.LeftShift);
            //OneUseItem
            characterActions.action4.AddDefaultBinding(InputControlType.Action3);
            characterActions.action4.AddDefaultBinding(Key.F);
        }
    }
}
