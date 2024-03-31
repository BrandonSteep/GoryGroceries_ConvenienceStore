using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventTrigger : MonoBehaviour
{
    public UnityEvent EventToCall;

    public void TriggerEvent(){
        EventToCall.Invoke();
    }
}
