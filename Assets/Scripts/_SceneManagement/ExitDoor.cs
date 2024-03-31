using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoor : MonoBehaviour
{
    [SerializeField] private ShoppingListInventory shoppingList;

    void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            SceneManager.LoadScene(2);
        }
    }
}
