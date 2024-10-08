using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveAction
{
    protected Character character;
    public MoveAction(Character _character) { character = _character; }
    public abstract void Execute(Vector3 direction);

}

