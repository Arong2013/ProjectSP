using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    Joystick joystick;
    Vector2 direction => joystick.Direction;
    public Player()
    {
        
    }
    public override void Init(GameObject gameObject)
    {
        base.Init(gameObject);
        joystick = Utils.GetUI<FloatingJoystick>();
        
    }
}
