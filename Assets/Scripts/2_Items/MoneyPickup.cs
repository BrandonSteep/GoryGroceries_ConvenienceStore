using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private ScriptableVariable amount;
    public void Interact()
    {
        ControllerReferences.playerWallet.AddMoney(amount.value);
        GetComponent<SmoothDampInterpolate>().BeginInterpolation(0.75f);
        Destroy(this.gameObject, 0.75f);
    }
}
