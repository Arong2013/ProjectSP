using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleState : EnemyHFSMState
{
    private Enemy Enemy;

    public IdleState(Enemy Enemy)
    {
        this.Enemy = Enemy;
    }

    public override void EnterState()
    {
    }

    public override void UpdateState()
    {
        // 플레이어와의 거리를 확인
        if (Enemy.IsPlayerInRange())
        {
            Enemy.stateMachine.ChangeState(Enemy.chaseState);
        }
    }

    public override void ExitState()
    {
        // 필요한 경우 상태를 종료할 때 처리
    }
}

public class ChaseState : EnemyHFSMState
{
    private Enemy Enemy;
    private NavMeshAgent agent;

    public ChaseState(Enemy Enemy)
    {
        this.Enemy = Enemy;
        agent = Enemy.GetComponent<NavMeshAgent>();
    }

    public override void EnterState()
    {
        agent.isStopped = false;
    }

    public override void UpdateState()
    {
        // 플레이어를 추적
        if (Enemy.IsPlayerInRange())
        {
            agent.SetDestination(Enemy.player.position);
        }
        else
        {
            // 플레이어가 범위를 벗어나면 Idle 상태로 전환
            Enemy.stateMachine.ChangeState(Enemy.idleState);
        }

        // 만약 공격 범위에 들어오면 Attack 상태로 전환
        if (Enemy.IsPlayerInAttackRange())
        {
            Enemy.stateMachine.ChangeState(Enemy.attackState);
        }
    }

    public override void ExitState()
    {
        agent.isStopped = true; // 추적 중지
    }
}

public class AttackState : EnemyHFSMState
{
    private Enemy Enemy;

    public AttackState(Enemy Enemy)
    {
        this.Enemy = Enemy;
    }

    public override void EnterState()
    {
    }

    public override void UpdateState()
    {
        // 공격 상태에서 일정 시간이 지나면 다시 추적 상태로 전환
        if (!Enemy.IsPlayerInAttackRange())
        {
            Enemy.stateMachine.ChangeState(Enemy.chaseState);
        }
    }

    public override void ExitState()
    {
        // 공격 상태 종료 시 처리
    }
}