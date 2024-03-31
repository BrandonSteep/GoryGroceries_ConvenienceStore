using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "State Machine/Decision/Blank")]
public class Decision_Blank : StateDecisionSO
{
        public override _State ChooseNextState(StateMachineController stateMachine){
            return stateMachine.CheckCurrentState();
        }
}
