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
        [SerializeField] private Animator animator;

        [Header("Movement Settings")]
        [SerializeField] private float walkSpeed = 6f;
        [SerializeField] private float sprintSpeed = 10f;
        [SerializeField] private float rotationSpeed = 12f;

        [Header("Jump Settings")]
        [SerializeField] private float jumpForce = 6f;
        [SerializeField] private float gravity = -20f;

        // Jump 안정화 요소
        private bool jumpBuffered;
        private float jumpBufferTimer;
        private readonly float jumpBufferTime = 0.15f;

        private float coyoteTimer;
        private readonly float coyoteTime = 0.12f;

        private float verticalVelocity;
        private bool isGrounded;

        [Header("Dash Settings")]
        [SerializeField] private float dashPower = 12f;
        [SerializeField] private float dashDuration = 0.2f;
        [SerializeField] private float dashCooldown = 1f;

        private bool isDashing = false;
        private bool canDash = true;
        public bool attackQueued = false;
        private Vector2 moveInput;
        private bool isSprinting;
        #endregion

        #region Unity Methods
        private void Update()
        {
            if (isDashing) return;

            UpdateGroundCheck();
            UpdateVerticalMovement();
            MovePlayer();
        }
        #endregion

        #region Input Methods
        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
        }
        public void OnAttack(InputAction.CallbackContext context)
        {
            if (!context.started) return;

            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

            bool isInAttackState = info.IsTag("Attack");

            if (!isInAttackState)
            {
                animator.SetTrigger("Attack"); // 수정됨
                attackQueued = false;
                return;
            }

            attackQueued = true;
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            // 점프 입력 버퍼
            jumpBuffered = true;
            jumpBufferTimer = jumpBufferTime;

            Debug.Log("Jump Buffered");
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
                TryDash();
        }
        #endregion

        #region Animator Method
        public void OnAttackAnimationEnd()
        {
            attackQueued = false;
        }
        #endregion

        #region Movement Logic
        private void UpdateGroundCheck()
        {
            if (controller.isGrounded)
            {
                isGrounded = true;
                coyoteTimer = coyoteTime;

                if (verticalVelocity < 0f)
                    verticalVelocity = -2f;
            }
            else
            {
                isGrounded = false;
                coyoteTimer -= Time.deltaTime;
            }
        }

        private void UpdateVerticalMovement()
        {
            // Jump buffer
            if (jumpBuffered)
            {
                jumpBufferTimer -= Time.deltaTime;
                if (jumpBufferTimer <= 0f)
                    jumpBuffered = false;
            }

            // 점프 조건: jumpBuffered + coyoteTime 안
            if (jumpBuffered && coyoteTimer > 0f)
            {
                verticalVelocity = jumpForce;
                jumpBuffered = false;

                if (animator != null)
                    animator.SetTrigger("Jump");
            }

            // 중력
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

            float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

            if (animator != null)
                animator.SetFloat("Speed", direction.magnitude * (isSprinting ? 2f : 1f), 0.1f, Time.deltaTime);

            Vector3 velocity = direction * currentSpeed;
            velocity.y = verticalVelocity;

            controller.Move(velocity * Time.deltaTime);
        }
        #endregion

        #region Dash Logic
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

            // 카메라 기준 이동 벡터 계산
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;
            camForward.y = 0; camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 dashDirection = camForward * moveInput.y + camRight * moveInput.x;

            if (dashDirection.sqrMagnitude < 0.01f)
                dashDirection = transform.forward;

            dashDirection.Normalize();

            float startTime = Time.time;

            while (Time.time < startTime + dashDuration)
            {
                controller.Move(dashDirection * dashPower * Time.deltaTime);
                yield return null;
            }

            isDashing = false;

            yield return new WaitForSeconds(dashCooldown);
            canDash = true;
        }
        #endregion
    }
}
