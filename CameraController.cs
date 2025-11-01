using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Systems.Camera
{
        /// <summary>
        /// Smoothly follows a target transform with a fixed offset.
        /// Use LateUpdate to ensure character movement has been processed first.
        /// </summary>
    public class CameraController : MonoBehaviour
    {
        [Tooltip("Target to follow (e.g. the player).")]
        public Transform target;


        [Tooltip("Offset from the target position.")]
        public Vector3 offset = new Vector3(0f, 5f, -10f);


        [Tooltip("Speed of camera smoothing.")]
        public float smoothSpeed = 5f;


        void LateUpdate()
        {
            if (target == null) 
            {
                return;
            }
            
                // Desired position is the target's position plus offset
            Vector3 desiredPosition = target.position + offset;
            
                // Smoothly interpolate to the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            
            transform.position = smoothedPosition;
            
                // Always look at the target
            transform.LookAt(target);
        }
    }
}