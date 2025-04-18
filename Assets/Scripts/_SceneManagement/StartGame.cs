using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField] private int sceneIndex = 1;
    public void TransitionScenes(){
        SceneManager.LoadScene(sceneIndex);
    }
}
