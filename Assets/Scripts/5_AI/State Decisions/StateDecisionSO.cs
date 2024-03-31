using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateDecisionSO : ScriptableObject
{
    public virtual _State ChooseNextState(StateMachineController stateMachine)
    {
        Debug.LogWarning("Using Blank State Decision - Create New Script Inhereting from this Class");
        throw new System.NotImplementedException();
    }
}
