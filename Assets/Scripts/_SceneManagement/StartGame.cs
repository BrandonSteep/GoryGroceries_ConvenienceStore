using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void TransitionScenes(){
        SceneManager.LoadScene(1);
    }
}
