using UnityEngine;
using UnityEngine.InputSystem;

namespace Choi
{
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CharacterController controller;
        [SerializeField] private Transform cameraTransform;

        [Header("Movement Settings")]
        [SerializeField] private float walkSpeed = 6f;
        [SerializeField] private float sprintSpeed = 10f;   // 스프린트 속도 추가
        [SerializeField] private float rotationSpeed = 12f;

        [Header("Jump Settings")]
        [SerializeField] private float jumpForce = 6f;
        [SerializeField] private float gravity = -20f;

        private Vector2 moveInput;
        private float verticalVelocity;
        private bool jumpRequested;
        private bool isGrounded;

        // Sprint
        private bool isSprinting = false;

        private void Update()
        {
            UpdateGroundCheck();
            UpdateVerticalMovement();
            MovePlayer();
        }

        /* Input System Callbacks */
        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                jumpRequested = true;
            }
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                isSprinting = true;   // shift 누르면 true
            }
            else if (context.canceled)
            {
                isSprinting = false;  // shift 떼면 false
            }
        }

        private void UpdateGroundCheck()
        {
            isGrounded = controller.isGrounded;

            if (isGrounded && verticalVelocity < 0f)
            {
                verticalVelocity = -2f;
            }
        }

        private void UpdateVerticalMovement()
        {
            if (jumpRequested && isGrounded)
            {
                verticalVelocity = jumpForce;
                jumpRequested = false;
            }

            verticalVelocity += gravity * Time.deltaTime;
        }

        private void MovePlayer()
        {
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;

            camForward.y = 0; camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 direction = camForward * moveInput.y + camRight * moveInput.x;

            // 회전
            if (direction.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
            }

            // *** 여기서 속도를 결정 ***
            float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

            Vector3 velocity = direction * currentSpeed;
            velocity.y = verticalVelocity;

            controller.Move(velocity * Time.deltaTime);
        }
    }
}
