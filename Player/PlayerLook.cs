using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private float sensitivity = 1.6f;
    [SerializeField] private float pitchMin = -80f, pitchMax = 80f;

    private float pitch;


    public void SetSensitivity(float tempSense)
    {
        sensitivity = Mathf.Max(0.05f, tempSense);
    }


    private void LateUpdate()
    {
        if (InputManager.Instance == null)
        {
            return;
        }
        
        
        Vector2 look = InputManager.Instance.lookInput;

        float dx = look.x * sensitivity;
        float dy = look.y * sensitivity;

            // Horizontal rotation (yaw)
        transform.Rotate(0f, dx, 0f, Space.Self);

            // Vertical rotation (pitch)
        pitch = Mathf.Clamp(pitch - dy, pitchMin, pitchMax);

        if (cameraPivot)
        {
            Vector3 angles = cameraPivot.localEulerAngles;
            angles.x = pitch;
            cameraPivot.localEulerAngles = angles;
        }
    }
}
