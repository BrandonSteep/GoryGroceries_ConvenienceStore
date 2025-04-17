using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(PlayerController))]
public class PlayerControllerEditor : Editor
{
    SerializedProperty walkSpeed;
    SerializedProperty aimWalkSpeedMultiplier;
    SerializedProperty currentWalkSpeed;
    SerializedProperty runSpeedMultiplier;
    SerializedProperty currentStamina;
    SerializedProperty maxStamina;
    SerializedProperty staminaDrainRate;
    SerializedProperty staminaRefillRate;
    SerializedProperty slopeForce;
    SerializedProperty slopeForceRayLength;
    SerializedProperty playerCamera;
    SerializedProperty mouseSensitivity;
    SerializedProperty lockCursor;
    SerializedProperty gravity;
    SerializedProperty moveSmoothTime;
    SerializedProperty mouseSmoothTime;
    SerializedProperty playerFov;
    SerializedProperty fovZoomAmount;
    SerializedProperty fovZoomTime;

    bool showMovement, showMouseLook = false;
    
    void OnEnable(){
        walkSpeed = serializedObject.FindProperty("walkSpeed");
        aimWalkSpeedMultiplier = serializedObject.FindProperty("aimWalkSpeedMultiplier");
        currentWalkSpeed = serializedObject.FindProperty("currentWalkSpeed");
        runSpeedMultiplier = serializedObject.FindProperty("runSpeedMultiplier");
        currentStamina = serializedObject.FindProperty("currentStamina");
        maxStamina = serializedObject.FindProperty("maxStamina");
        staminaDrainRate = serializedObject.FindProperty("staminaDrainRate");
        staminaRefillRate = serializedObject.FindProperty("staminaRefillRate");
        slopeForce = serializedObject.FindProperty("slopeForce");
        slopeForceRayLength = serializedObject.FindProperty("slopeForceRayLength");
        mouseSensitivity = serializedObject.FindProperty("mouseSensitivity");
        gravity = serializedObject.FindProperty("gravity");
        moveSmoothTime = serializedObject.FindProperty("moveSmoothTime");
        mouseSmoothTime = serializedObject.FindProperty("mouseSmoothTime");
        playerFov = serializedObject.FindProperty("playerFov");
        fovZoomAmount = serializedObject.FindProperty("fovZoomAmount");
        fovZoomTime = serializedObject.FindProperty("fovZoomTime");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        showMovement = EditorGUILayout.BeginFoldoutHeaderGroup(showMovement, "Movement");
        if(showMovement){
            EditorGUILayout.PropertyField(walkSpeed);
            EditorGUILayout.PropertyField(aimWalkSpeedMultiplier);
            EditorGUILayout.PropertyField(currentWalkSpeed);
            EditorGUILayout.PropertyField(runSpeedMultiplier);
            EditorGUILayout.PropertyField(currentStamina);
            EditorGUILayout.PropertyField(maxStamina);
            EditorGUILayout.PropertyField(staminaDrainRate);
            EditorGUILayout.PropertyField(staminaRefillRate);
            EditorGUILayout.PropertyField(slopeForce);
            EditorGUILayout.PropertyField(slopeForceRayLength);
            EditorGUILayout.PropertyField(gravity);
            EditorGUILayout.PropertyField(moveSmoothTime);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        
        showMouseLook = EditorGUILayout.BeginFoldoutHeaderGroup(showMouseLook, "MouseLook");
        if(showMouseLook){
            EditorGUILayout.PropertyField(mouseSensitivity);
            EditorGUILayout.PropertyField(mouseSmoothTime);
            EditorGUILayout.PropertyField(playerFov);
            EditorGUILayout.PropertyField(fovZoomAmount);
            EditorGUILayout.PropertyField(fovZoomTime);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        serializedObject.ApplyModifiedProperties();
    }
}
#endif