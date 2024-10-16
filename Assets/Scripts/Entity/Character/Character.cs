using System.Collections;
using System.Collections.Generic;
using DungeonArchitect.Samples.GridFlow;
using UnityEngine;

public abstract class Character : Entity
{
    CombatStats combatStats = new CombatStats();
    [SerializeField] protected CharacterStateMachine characterStateMachine = new CharacterStateMachine();
    protected Animator animator;
    protected Rigidbody rb;
    string currentAnimeState = default;

    public override void Init()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        animator = gameObject.GetComponent<Animator>();

        if (rb == null || animator == null)
        {
            Debug.LogError("Required components missing on character!");
        }
    }

    protected bool IsAnimatorable() => animator != null;

    protected void SimpleMove(Vector3 direction)
    {
        if (!IsAnimatorable()) return;

        // Z축 값은 0으로 고정하여 위아래로 움직이지 않도록 처리
        direction.z = 0;

        float directionMagnitude = direction.magnitude;
        string targetAnimation = directionMagnitude == 0 ? "Idle" : (directionMagnitude < 0.5f ? "Walk" : "Sprint");
        float baseSpeed = directionMagnitude < 0.5f ? 0.5f : 1f;

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(targetAnimation) && currentAnimeState != targetAnimation)
        {
            currentAnimeState = targetAnimation;
            animator.CrossFade(targetAnimation, 0.1f);
        }

        rb.MovePosition(rb.position + direction * combatStats.speed.Value * baseSpeed * Time.deltaTime);

        // 회전 처리 (Z축 회전은 무시, 평면상 회전만)
        if (directionMagnitude > 0.1f) 
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    public void MeleeAttack(MeleeWeaponController weaponController)
    {
        if (characterStateMachine.AddState(CharacterState.Attacking))
        {
            StartCoroutine(HandleWeaponCollider());

            IEnumerator HandleWeaponCollider()
            {
                if (!IsAnimatorable()) yield break;

                animator.CrossFade("MeleeAttack", 0.1f);
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                yield return new WaitForSeconds(stateInfo.length * 0.2f);
                weaponController.EnableCollider(true);

                while (stateInfo.normalizedTime < 0.8f)
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).fullPathHash != stateInfo.fullPathHash)
                    {
                        weaponController.EnableCollider(false);
                        yield break;
                    }
                    stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                    yield return null;
                }
                weaponController.EnableCollider(false);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (characterStateMachine.AddState(CharacterState.Damgeing))
        {
            combatStats.currentHP.AddModifier(new StatModifier(-damage, StatModType.Flat));
            if (IsAnimatorable())
            {
                animator.CrossFade("TakeDamage", 0.1f);
            }
        }
    }
}

