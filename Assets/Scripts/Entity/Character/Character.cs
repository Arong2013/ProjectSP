using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : Entity
{
    protected Animator animator;
    protected Rigidbody rigidbody;
    public override void Init(GameObject gameObject)
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
        animator = gameObject.GetComponent<Animator>();
    }
    protected bool IsAnimatorable()
    {
        return true;
    }
}
