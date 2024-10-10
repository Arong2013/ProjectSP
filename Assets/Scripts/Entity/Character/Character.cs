using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : Entity
{
    CombatStats combatStats = new CombatStats();
    protected CharacterStateMachine characterStateMachine = new CharacterStateMachine();
    protected Animator animator;
    protected Rigidbody rigidbody;
    string currentAnimeState = default;
    
    public override void Init(GameObject gameObject)
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
        animator = gameObject.GetComponent<Animator>();
    }
    protected bool IsAnimatorable()
    {
        return true;
    }
    protected void SimpleMove(Vector3 direction)
    {
        float directionMagnitude = direction.magnitude;
        string targetAnimation = directionMagnitude == 0 ? "Idle" : (directionMagnitude < 0.5f ? "Walk" : "Sprint");
        float baseSpeed = directionMagnitude < 0.5f ? 0.5f : 1f;
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(targetAnimation) && currentAnimeState != targetAnimation)
        {
            currentAnimeState = targetAnimation;
            animator.CrossFade(targetAnimation, 0.1f);
        }
        rigidbody.MovePosition(rigidbody.position + direction * combatStats.speed.Value * baseSpeed * Time.deltaTime);
        // 회전 처리 (이동하는 방향으로)
        if (directionMagnitude > 0.1f) // 방향이 있는 경우에만 회전
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            rigidbody.rotation = Quaternion.Slerp(rigidbody.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }
}
