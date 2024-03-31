using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwappableStatus : MonoBehaviour
{
    public bool _canSwap = true;

    public void DisableSwap(){
        if(_canSwap == true){
            _canSwap = false;
        }
    }

    public void EnableSwap(){
        if(_canSwap != true){
            _canSwap = true;
        }
    }
}
