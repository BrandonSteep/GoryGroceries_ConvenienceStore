using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static List<ShoppingItem> currentItems = new List<ShoppingItem>();
    public static int currentPoints;

    void Awake(){
        if (!instance){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }

    // void Update(){
    //     Debug.Log(currentItems.Count);
    // }
}
