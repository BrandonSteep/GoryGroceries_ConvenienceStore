using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI(){
        FieldOfView fov = (FieldOfView)target;
        Handles.color = Color.white;

        var fovPosition = fov.transform.position + Vector3.up * 1f;

        Handles.DrawWireArc(fovPosition, Vector3.up, Vector3.forward, 360, fov.CheckRadius());
    
        Vector3 viewAngle01 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.CheckAngle() / 2);
        Vector3 viewAngle02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.CheckAngle() / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(fovPosition, fovPosition + viewAngle01 * fov.CheckRadius());
        Handles.DrawLine(fovPosition, fovPosition + viewAngle02 * fov.CheckRadius());

        if(fov.CheckPlayerSight()){
            Handles.color = Color.yellow;
            Handles.DrawLine(fovPosition, ControllerReferences.player.transform.position);
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees){
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
