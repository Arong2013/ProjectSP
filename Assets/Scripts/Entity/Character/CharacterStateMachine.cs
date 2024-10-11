using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public enum CharacterState
{
    None,       // 기본 상태
    Moving,     // 이동 중
    Attacking,  // 공격 중
    Reloading,  // 리로드 중
    Dodging,    // 회피 중
    Blocking,   // 방어 중
    Healing,     // 회복 중
    Damgeing
}

[System.Serializable]
public class CharacterStateMachine
{
    // 현재 활성화된 상태를 저장하는 HashSet
    [ShowInInspector, ReadOnly]
    private HashSet<CharacterState> activeStates = new HashSet<CharacterState>();

    // 상태별 우선순위를 정의하는 딕셔너리 (Odin Inspector에서 편집 가능하게 표시)
    [ShowInInspector,OdinSerialize, DictionaryDrawerSettings(KeyLabel = "State", ValueLabel = "Priority")]
    Dictionary<CharacterState, int> statePriority;
    public bool AddState(CharacterState newState)
    {
        int currentHighestPriority = GetHighestPriority();
        int newStatePriority = statePriority[newState];
        if (newStatePriority > currentHighestPriority)
        {
            RemoveLowerPriorityStates(newStatePriority);
        }
        if (newStatePriority >= currentHighestPriority)
        {
            activeStates.Add(newState);
            Debug.Log($"{newState} state added.");
            return true;
        }
        else
        {
            Debug.Log($"Cannot add {newState} state. A higher priority state is active.");
            return false;
        }
    }

    // 우선순위가 더 낮은 상태들을 제거하는 메서드
    private void RemoveLowerPriorityStates(int priority)
    {
        var statesToRemove = activeStates.Where(state => statePriority[state] < priority).ToList();

        foreach (var state in statesToRemove)
        {
            activeStates.Remove(state);
            Debug.Log($"{state} state removed due to higher priority state.");
        }
    }

    // 현재 활성 상태 중에서 가장 높은 우선순위를 반환 (LINQ 사용)
    private int GetHighestPriority()
    {
        if (activeStates.Count == 0)
        {
            return 0;
        }

        return activeStates.Max(state => statePriority[state]);
    }

    // 상태 제거
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

    // 특정 상태가 활성화 되어있는지 확인
    public bool HasState(CharacterState state)
    {
        return activeStates.Contains(state);
    }

    // 활성화된 모든 상태 보기 (Odin Inspector에서 버튼으로 출력)
    [Button("Print Active States")]
    private void PrintActiveStates()
    {
        string activeStateString = activeStates.Count > 0 
            ? string.Join(", ", activeStates) 
            : "No active states";

        Debug.Log($"Active States: {activeStateString}");
    }
}
