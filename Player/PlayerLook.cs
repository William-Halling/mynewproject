using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Transform cameraPivot; // child at eye height
    [SerializeField] private float sensitivity = 1.6f;
    [SerializeField] private float pitchMin = -80f, pitchMax = 80f;


    private float pitch;

    public void SetSensitivity(float tempSense) => sensitivity = Mathf.Max(0.05f, tempSense);


    private void LateUpdate()
    {
        if (InputManager.Instance == null) 
            
            return;
        
        var look = InputManager.Instance.LookInput;

        
        // Multiply by sensitivity (tune as needed)
        float dx = look.x * sensitivity;
        float dy = look.y * sensitivity;


        // Yaw on body
        transform.Rotate(0f, dx, 0f, Space.Self);


        // Pitch on camera pivot
        pitch = Mathf.Clamp(pitch - dy, pitchMin, pitchMax);
        if (cameraPivot)
        {
            var r = cameraPivot.localEulerAngles;

            r.x = pitch;
            
            cameraPivot.localEulerAngles = r;
        }
    }
}
