using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class MyCharacterActions : PlayerActionSet
{
    public PlayerAction left;
    public PlayerAction right;
    public PlayerAction up;
    public PlayerAction down;

    // like this, or name them afte the action the player performs.
    public PlayerAction action1;
    public PlayerAction action2;
    public PlayerAction action3;
    public PlayerAction action4;
    public PlayerAction l1; 
    public PlayerAction l2;
    public PlayerAction l3;
    public PlayerAction r1;
    public PlayerAction r2;
    public PlayerAction r3;
    public PlayerAction start;
    public PlayerAction record;
    
    public PlayerTwoAxisAction Direction;

    public MyCharacterActions()
    {
        left = CreatePlayerAction("Left");
        right = CreatePlayerAction("Right");
        up = CreatePlayerAction("Up");
        down = CreatePlayerAction("Down");
       
        Direction = CreateTwoAxisPlayerAction(right, left, down, up);
    }      
}
