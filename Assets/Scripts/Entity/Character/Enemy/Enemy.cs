using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    public float detectionRange = 10f;
    public float attackRange = 2f;
    private NavMeshAgent agent;
    public override void Init()
    {
        base.Init();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        Init();
    }

    void Update()
    {
        characterStateMachine.Update(); // 상태 머신 업데이트
    }

    // 애니메이션 전환 처리
    public void SetAnimation(string animationName)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animationName)) return;
        animator.CrossFade(animationName, 0.1f);
    }
    public override bool IsFOVInRange(Transform target)
    {
        float distance = Vector3.Distance(target.position, transform.position);
        return distance <= detectionRange;
    }
    public override bool IsInAttackRange(Transform target)
    {
        float distance = Vector3.Distance(target.position, transform.position);
        return distance <= attackRange;
    }
}
