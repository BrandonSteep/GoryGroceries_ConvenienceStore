using UnityEngine;

public class SkyboxChanger : MonoBehaviour
{
    [SerializeField] private Material _skyboxMat1;
    [SerializeField] private Material _skyboxMat2;

    public void SwapSkybox(){
        Debug.Log("Swapping Skybox");

        if(RenderSettings.skybox == _skyboxMat2){
            RenderSettings.skybox = _skyboxMat1;
        }
        else if(RenderSettings.skybox == _skyboxMat1){
            RenderSettings.skybox = _skyboxMat2;
        }
        else{
            return;
        }
    }
}