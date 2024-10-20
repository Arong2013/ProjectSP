using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterStateMachine
{
    private HFSMState currentState;

    public void Initialize(HFSMState startingState)
    {
        currentState = startingState;
        startingState.EnterState();
    }

    public void ChangeState(HFSMState newState)
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