using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public enum CharacterState
{
    None, Moving, Attacking, Reloading, Dodging, Blocking, Healing, Damgeing
}

[System.Serializable]
public class CharacterStateMachine
{
    [ShowInInspector, ReadOnly]
    private HashSet<CharacterState> activeStates = new HashSet<CharacterState>();

    [ShowInInspector, OdinSerialize, DictionaryDrawerSettings(KeyLabel = "State", ValueLabel = "Priority")]
    Dictionary<CharacterState, int> statePriority;

    public bool AddState(CharacterState newState)
    {
        int currentHighestPriority = GetHighestPriority();
        int newStatePriority = statePriority[newState];

        if (newStatePriority >= currentHighestPriority)
        {
            RemoveLowerPriorityStates(newStatePriority);
            activeStates.Add(newState);
            Debug.Log($"{newState} state added.");
            return true;
        }
        Debug.Log($"Cannot add {newState} state. A higher priority state is active.");
        return false;
    }

    private void RemoveLowerPriorityStates(int priority)
    {
        var statesToRemove = activeStates.Where(state => statePriority[state] < priority).ToList();
        foreach (var state in statesToRemove)
        {
            activeStates.Remove(state);
            Debug.Log($"{state} state removed due to higher priority state.");
        }
    }

    private int GetHighestPriority() => activeStates.Count == 0 ? 0 : activeStates.Max(state => statePriority[state]);

    public void RemoveState(CharacterState stateToRemove)
    {
        if (activeStates.Remove(stateToRemove))
        {
            Debug.Log($"{stateToRemove} state removed.");
        }
        else
        {
            Debug.Log($"State {stateToRemove} is not active.");
        }
    }

    public bool HasState(CharacterState state) => activeStates.Contains(state);

    [Button("Print Active States")]
    private void PrintActiveStates()
    {
        string activeStateString = activeStates.Count > 0 ? string.Join(", ", activeStates) : "No active states";
        Debug.Log($"Active States: {activeStateString}");
    }
}
