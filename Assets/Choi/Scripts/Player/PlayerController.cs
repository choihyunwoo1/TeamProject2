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
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float rotationSpeed = 12f;

        [Header("Jump Settings")]
        [SerializeField] private float jumpForce = 6f;
        [SerializeField] private float gravity = -20f;

        private Vector2 moveInput;
        private float verticalVelocity;
        private bool jumpRequested;
        private bool isGrounded;

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

        private void UpdateGroundCheck()
        {
            isGrounded = controller.isGrounded;

            if (isGrounded && verticalVelocity < 0f)
            {
                // CharacterController의 바닥 체크 안정화
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

            // 중력 적용
            verticalVelocity += gravity * Time.deltaTime;
        }

        private void MovePlayer()
        {
            // 카메라 기반 방향
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;

            camForward.y = 0; camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 direction = camForward * moveInput.y + camRight * moveInput.x;

            // 회전 처리
            if (direction.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
            }

            // 최종 이동 벡터
            Vector3 velocity = direction * moveSpeed;
            velocity.y = verticalVelocity;

            // CharacterController는 Move 한 번만!
            controller.Move(velocity * Time.deltaTime);
        }
    }
}
