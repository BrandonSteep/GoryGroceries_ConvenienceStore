using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieNavigation : MonoBehaviour
{
    private NavMeshAgent nav;
    
    void Awake(){
        nav = GetComponent<NavMeshAgent>();
    }

    void Update(){
        nav.SetDestination(ControllerReferences.player.transform.position);
    }
}
