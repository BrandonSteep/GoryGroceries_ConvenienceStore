using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    public ScriptableVariable currentHp;
    public ScriptableVariable maxHp;
    public bool isAlive = true;

    public bool canTakeDamage = true;

    void Awake()
    {
        currentHp.value = maxHp.value;
    }
}
