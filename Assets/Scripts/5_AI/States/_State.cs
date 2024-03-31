using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _State : ScriptableObject
{
    [SerializeField] private StateDecisionSO stateDecision;

    public virtual _State RunState(StateMachineController stateMachine)
    {
        return stateDecision.ChooseNextState(stateMachine);
    }

    public virtual void EnterState(StateMachineController stateMachine){}
}
