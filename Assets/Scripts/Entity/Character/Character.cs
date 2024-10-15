using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : Entity
{
    CombatStats combatStats = new CombatStats();
    [SerializeField] protected CharacterStateMachine characterStateMachine = new CharacterStateMachine();
    protected Animator animator;
    protected Rigidbody rigidbody;
    string currentAnimeState = default;

    public override void Init()
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

        // Z축이 고정된 방향으로 이동 처리
        rigidbody.MovePosition(rigidbody.position + direction * combatStats.speed.Value * baseSpeed * Time.deltaTime);

        // 회전 처리 (Z축 회전은 무시, 평면상 회전만)
        if (directionMagnitude > 0.1f) // 방향이 있는 경우에만 회전
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            rigidbody.rotation = Quaternion.Slerp(rigidbody.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }


    public void MeleeAttack(MeleeWeaponController weaponContoller)
    {
        if (characterStateMachine.AddState(CharacterState.Attacking))
        {
            StartCoroutine(HandleWeaponCollider());
        }

        IEnumerator HandleWeaponCollider()
        {
            animator.CrossFade("MeleeAttack", 0.1f);
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForSeconds(stateInfo.length * 0.2f);
            weaponContoller.EnableCollider(true);
            while (stateInfo.normalizedTime < 0.8f)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).fullPathHash != stateInfo.fullPathHash)
                {
                    weaponContoller.EnableCollider(false);
                    yield break;
                }
                stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                yield return null; // 다음 프레임까지 대기
            }
            weaponContoller.EnableCollider(false);
        }
    }
    public void TakeDamage(float damge)
    {
        if (characterStateMachine.AddState(CharacterState.Damgeing))
        {
            combatStats.currentHP.AddModifier(new StatModifier(-damge, StatModType.Flat));
            animator.CrossFade("TakeDamge", 0.1f);
        }
    }
}
