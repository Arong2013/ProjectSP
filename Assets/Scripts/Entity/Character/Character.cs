using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : Entity
{
    Rigidbody rigidbody;
    public override void Init(GameObject gameObject)
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
    }
}
