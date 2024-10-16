using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    public Transform player;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    private NavMeshAgent agent;

    public EnemyStateMachine stateMachine;
    public IdleState idleState;
    public ChaseState chaseState;
    public AttackState attackState;

    public override void Init()
    {
        base.Init();
        agent = GetComponent<NavMeshAgent>();

        // 상태 초기화
        idleState = new IdleState(this);
        chaseState = new ChaseState(this);
        attackState = new AttackState(this);

        stateMachine = new EnemyStateMachine();
        stateMachine.Initialize(idleState); // 초기 상태는 Idle
    }

    private void Start()
    {
        Init();
    }

    void Update()
    {
        stateMachine.Update(); // 상태 머신 업데이트
    }

    // 애니메이션 전환 처리
    public void SetAnimation(string animationName)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animationName)) return;
        animator.CrossFade(animationName, 0.1f);
    }

    // 플레이어와의 거리 확인 (추적 거리 확인)
    public bool IsPlayerInRange()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        return distance <= detectionRange;
    }

    // 공격 범위 확인
    public bool IsPlayerInAttackRange()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        return distance <= attackRange;
    }
}
