using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

namespace Choi
{
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        [Header("References")]
        [SerializeField] private CharacterController controller;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private Animator animator;  // 애니메이터 추가

        [Header("Movement Settings")]
        [SerializeField] private float walkSpeed = 6f;
        [SerializeField] private float sprintSpeed = 10f;
        [SerializeField] private float rotationSpeed = 12f;

        [Header("Jump Settings")]
        [SerializeField] private float jumpForce = 6f;
        [SerializeField] private float gravity = -20f;

        [Header("Dash Settings")]
        [SerializeField] private float dashPower = 12f;       // 대쉬 속도
        [SerializeField] private float dashDuration = 0.2f;   // 대쉬 유지 시간
        [SerializeField] private float dashCooldown = 1f;     // 쿨타임

        private bool isDashing = false;
        private bool canDash = true;

        private Vector2 moveInput;
        private float verticalVelocity;
        private bool jumpRequested;
        private bool isGrounded;
        private bool isSprinting;
        #endregion

        #region Unity Event Method
        private void Update()
        {
            if (isDashing) return; // 대쉬 중이면 일반 이동/점프 중단

            UpdateGroundCheck();
            UpdateVerticalMovement();
            MovePlayer();
        }
        #endregion

        #region Custom Method
        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed)
                jumpRequested = true;
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            if (context.performed)
                isSprinting = true;
            else if (context.canceled)
                isSprinting = false;
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                TryDash();
            }
        }

        /* Movement Logic */

        private void UpdateGroundCheck()
        {
            isGrounded = controller.isGrounded;

            if (isGrounded && verticalVelocity < 0f)
                verticalVelocity = -2f;
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

            camForward.y = 0;
            camRight.y = 0;

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

            // 속도
            float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

            // BlendTree용 Speed 파라미터 전달
            if (animator != null)
                animator.SetFloat("Speed", direction.magnitude * (isSprinting ? 2f : 1f), 0.1f, Time.deltaTime);

            // 최종 이동 벡터
            Vector3 velocity = direction * currentSpeed;
            velocity.y = verticalVelocity;

            controller.Move(velocity * Time.deltaTime);
        }

        /* DASH 기능 */

        private void TryDash()
        {
            if (!canDash || isDashing) return;

            StartCoroutine(DashRoutine());
        }

        private IEnumerator DashRoutine()
        {
            isDashing = true;
            canDash = false;

            if (animator != null)
                animator.SetTrigger("Dash");

            // 1) 카메라 기준 이동 벡터 계산
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;
            camForward.y = 0; camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            // 2) 입력값 기반 대시 방향 계산
            Vector3 dashDirection = camForward * moveInput.y + camRight * moveInput.x;

            // 3) 이동 입력이 전혀 없으면 정면으로 대시
            if (dashDirection.sqrMagnitude < 0.01f)
                dashDirection = transform.forward;

            dashDirection.Normalize();

            float startTime = Time.time;

            // 4) 대시 구간 동안 중력 적용 없이 이동
            while (Time.time < startTime + dashDuration)
            {
                controller.Move(dashDirection * dashPower * Time.deltaTime);
                yield return null;
            }

            // 5) 대시 종료
            isDashing = false;

            // 6) 쿨타임 후 다시 가능
            yield return new WaitForSeconds(dashCooldown);
            canDash = true;
        }
        #endregion
    }
}
