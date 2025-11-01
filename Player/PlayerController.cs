using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
        /// <summary>
        /// Controls the player character: movement, sprinting, and jumping.
        /// Uses CharacterController to move with collision detection:contentReference[oaicite:6]{index=6}.
        /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
            // Public tunable parameters
        [SerializeField] private float walkSpeed    = 5f;
        [SerializeField] private float sprintSpeed  = 8f;
        [SerializeField] private float jumpForce    = 8f;
        [SerializeField] private float gravity      = 20f;
        [SerializeField] private float acceleration = 10f;


        private CharacterController controller;
        private Vector3 velocity = Vector3.zero;
        private float currentSpeed;
        private bool wasGroundedLastFrame;


        private void Awake()
        {
            controller = GetComponent<CharacterController>();
        }


        private void Start()
        {
            currentSpeed = walkSpeed;
        }


        private void Update()
        {
            if (InputManager.Instance == null) return;

            Vector2 moveInput = InputManager.Instance.moveInput;
            bool isSprinting  = InputManager.Instance.isSprinting;
            bool isJumping    = InputManager.Instance.isJumping;


            float targetSpeed = isSprinting ? sprintSpeed : walkSpeed;
            currentSpeed      = Mathf.Lerp(currentSpeed, targetSpeed, acceleration * Time.deltaTime);


            Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
            Vector3 horizontalVelocity = move * currentSpeed;


            if (controller.isGrounded)
            {
                if (!wasGroundedLastFrame)
                {
                    velocity.y = -1f;
                }

                if (isJumping)
                {
                    velocity.y = jumpForce;
                }
            }
            else
            {
                velocity.y -= gravity * Time.deltaTime;
            }


            wasGroundedLastFrame = controller.isGrounded;


            Vector3 finalVelocity = horizontalVelocity + Vector3.up * velocity.y;
            controller.Move(finalVelocity * Time.deltaTime);
        }
    }
}