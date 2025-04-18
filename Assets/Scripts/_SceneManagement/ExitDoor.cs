using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoor : MonoBehaviour
{
    [SerializeField] private GameEvent exitEvent;

    void OnTriggerEnter(Collider other){
        exitEvent.Raise();
        
        if(other.tag == "Player"){
            SceneManager.LoadScene(3);
        }
    }
}
