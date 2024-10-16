using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyStateMachine
{
    private EnemyHFSMState currentState;

    public void Initialize(EnemyHFSMState startingState)
    {
        currentState = startingState;
        startingState.EnterState();
    }

    public void ChangeState(EnemyHFSMState newState)
    {
        currentState.ExitState();
        currentState = newState;
        newState.EnterState();
    }

    public void Update()
    {
        currentState?.UpdateState();
    }
}