using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleState : HFSMState
{
    private Character character;

    public IdleState(Character character)
    {
        this.character = character;
    }

    public override void EnterState()
    {
    }

    public override void UpdateState()
    {

    }
    public override void ExitState()
    {
        // 필요한 경우 상태를 종료할 때 처리
    }
}
public class ChaseState : HFSMState
{
    private Character character;
    private NavMeshAgent agent;

    private Transform target;
    public ChaseState(Character character, Transform transform)
    {
        this.character = character;
        this.target = transform;
        agent = character.GetComponent<NavMeshAgent>();
    }

    public override void EnterState()
    {
        agent.isStopped = false;
    }

    public override void UpdateState()
    {
        // 플레이어를 추적
        if (character.IsFOVInRange(target))
        {
            agent.SetDestination(target.position);
        }
        else
        {
            character.characterStateMachine.ChangeState(new IdleState(character));
        }
        if (character.IsInAttackRange(target))
        {
            character.characterStateMachine.ChangeState(new AttackState(character,target));
        }
    }

    public override void ExitState()
    {
        agent.isStopped = true; // 추적 중지
    }
}
public class AttackState : HFSMState
{
    private Character character;
    private Transform target;
    public AttackState(Character character,Transform target)
    {
        this.character = character;
        this.target = target;
    }

    public override void EnterState()
    {
    }

    public override void UpdateState()
    {
        if (!character.IsInAttackRange(target))
        {
            character.characterStateMachine.ChangeState(new ChaseState(character,target));
        }
    }

    public override void ExitState()
    {
        // 공격 상태 종료 시 처리
    }
}
