using UnityEngine;

public class ToggleGameObject : MonoBehaviour
{
    [SerializeField] GameObject objectToToggle;
    public void Toggle(){
        objectToToggle.SetActive(!objectToToggle.activeInHierarchy);
    }
}
