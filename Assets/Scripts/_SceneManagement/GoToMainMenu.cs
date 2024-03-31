using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToMainMenu : MonoBehaviour
{
    public void TransitionScenes(){
        SceneManager.LoadScene(0);
    }
}
