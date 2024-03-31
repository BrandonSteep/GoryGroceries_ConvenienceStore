using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockMouseCursor : MonoBehaviour
{
    void OnEnable(){
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
    }
}
