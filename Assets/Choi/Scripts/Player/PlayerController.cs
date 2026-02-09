using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using HJ; 

namespace Choi
{
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        [Header("References")]
        [SerializeField] private CharacterController controller;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private Animator animator;
        private PlayerStats stats;

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

        //Moving Platform 안정화 요소
        private AnimatedPlatform currentPlatform;
        private Vector3 platformLocalOffset;
        private bool onPlatform = false;

        private bool initialized = false;

        private float coyoteTimer;
        private readonly float coyoteTime = 0.12f;

        private float verticalVelocity;

        [Header("Dash Settings")]
        [SerializeField] private float dashPower = 12f;
        [SerializeField] private float dashDuration = 0.2f;
        [SerializeField] private float dashCooldown = 1f;

        [Header("HeavyAttack Settings")]
        [SerializeField] private float heavyAttackHoldTime = 0.5f;
        private float attackButtonHeldTime = 0f;

        [Header("UltimateAttack Settings")]
        private bool canUltimate = true;
        [SerializeField] private float ultimateCooldown = 30f;
        [SerializeField] private GameObject ultimatePortalPrefab;
        [SerializeField] private Transform ultimateSpawnPoint;

        public WeaponHitbox weaponHitbox;

        private bool isDashing = false;
        private bool canDash = true;
        public bool attackQueued = false;
        private Vector2 moveInput;
        private bool isSprinting;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            stats = GetComponent<PlayerStats>();
        }
        private void Start()
        {
            // 모든 필드 초기화 끝난 후 마지막에 true로
            initialized = true;
        }
        private void Update()
        {
            if (stats.IsDead) return; // 죽었으면 움직임 중지
            if (isDashing) return;

            UpdateVerticalMovement();
            MovePlayer();      // Move() 호출
            UpdateGroundCheck();  // Move 후에 체크해야 isGrounded가 정확함
            HandleAttackInput();
        }

        //CharacterController 전용 이벤트 메서드
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            AnimatedPlatform platform = hit.collider.GetComponentInParent<AnimatedPlatform>();
            if (platform != null)
            {
                if (currentPlatform != platform)
                {
                    currentPlatform = platform;
                    onPlatform = true;
                    platformLocalOffset = transform.position - platform.transform.position;
                }
            }
            else
            {
                onPlatform = false;
                currentPlatform = null;
            }
        }
        private void LateUpdate()
        {
            if (onPlatform && currentPlatform != null)
            {
                controller.Move(currentPlatform.DeltaMovement);
            }
        }
        #endregion

        #region Input Methods
        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
            bool isInputOn = moveInput.magnitude > 0;

            //HJ : 걷는소리
            if (isInputOn)
            {
                SoundManager.Instance.Play("Walk");
            }
            else if (!isInputOn)
            {
                SoundManager.Instance.Stop("Walk");
            }

        }
        public void OnAttack(InputAction.CallbackContext context)
        {
            if (!initialized) return; // 초기화 전에는 공격 입력 무시
            if (stats.IsDead) return;
            if (!context.started) return;

            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(1);

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

        public void OnUlitimate(InputAction.CallbackContext context)
        {
            if (context.performed)
                UltimateAttack();
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
                coyoteTimer = coyoteTime;

                if (verticalVelocity < 0f)
                    verticalVelocity = -2f;
            }
            else
            {
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
                if (stats.ConsumeStamina(5f))
                {
                    verticalVelocity = jumpForce;
                    jumpBuffered = false;

                    if (animator != null)
                        animator.SetTrigger("Jump");

                    //HJ : 점프 조건 맞을때 사운드 재생 (1회)
                    SoundManager.Instance.Play("Jump");
                }
                else
                {
                    jumpBuffered = false; // 스태미너 부족 → 점프취소
                }
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

            float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

            if (isSprinting && direction.sqrMagnitude > 0.1f)
            {
                stats.ConsumeStamina(Time.deltaTime * 8f);
                //HJ : 달릴때 소리 1.6배속
                SoundManager.Instance.SetPitch("Walk", 1.6f);
            }
            else
            {
                //HJ : 아니면 소리 1로 재설정
                SoundManager.Instance.SetPitch("Walk", 1f);
            }

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

            //HJ : Dash 사운드
            SoundManager.Instance.Play("Dash");

            if (!stats.ConsumeStamina(20f))
            {
                //HJ : Dash 사운드 스톱
                SoundManager.Instance.Stop("Dash");
                return; // ← 스태미너 부족하면 대시 안됨
            }

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

        #region Attack
        private void HandleAttackInput()
        {
            // 마우스 눌림
            if (Mouse.current.leftButton.isPressed)
            {
                attackButtonHeldTime += Time.deltaTime;

                // 0.5초 이상 → 강공격 발동
                if (attackButtonHeldTime >= heavyAttackHoldTime)
                {
                    float heavyCost = 10f; // 강공격 게이지 소모량

                    if (stats.ConsumeGauge(heavyCost))
                    {
                        animator.SetTrigger("HeavyAttack");
                        animator.SetBool("IsAttacking", false);
                    }
                    else
                    {
                        animator.SetBool("IsAttacking", false);

                        Debug.Log("Need Gauge");
                        // 게이지가 부족하면 강공격 취소하고 경공격으로 처리하거나 무효 처리
                    }

                    attackButtonHeldTime = 0f;
                    return;
                }


                // 마우스 누르는 동안 루프 재생 (이미 true면 Set 안함)
                if (!animator.GetBool("IsAttacking"))
                {
                    animator.SetBool("IsAttacking", true);
                }
            }
            else
            {
                // 마우스 떼면 루프 종료
                if (animator.GetBool("IsAttacking"))
                {
                    animator.SetBool("IsAttacking", false);
                }
                attackButtonHeldTime = 0f;
            }
        }
        #endregion

        #region Ultimate
        private IEnumerator UltimateCooldown()
        {
            canUltimate = false;
            yield return new WaitForSeconds(ultimateCooldown);
            canUltimate = true;
        }
        private void UltimateAttack()
        {
            if (!canUltimate) return;

            if (!stats.ConsumeGauge(100f))
                return;

            animator.SetTrigger("Ultimate");

            SpawnUltimatePortal();

            StartCoroutine(UltimateCooldown());
        }
        private void SpawnUltimatePortal()
        {
            if (ultimatePortalPrefab == null)
            {
                Debug.LogWarning("Ultimate Portal Prefab is not assigned!");
                return;
            }

            // 스폰 위치: 지정 지점이 있으면 그것, 없으면 플레이어 발밑
            Vector3 spawnPos = ultimateSpawnPoint != null
                ? ultimateSpawnPoint.position
                : transform.position + Vector3.down * 0.1f;

            Quaternion spawnRot = Quaternion.identity;

            var portal = Instantiate(ultimatePortalPrefab, spawnPos, spawnRot);
            Destroy(portal, 1.4f);
        }
        #endregion

        #region AttackType
        // 기본 공격
        public void StartNormalAttack()
        {
            weaponHitbox.damageType = DamageType.Normal;
            weaponHitbox.baseDamage = 10f;
        }

        // 강공격
        public void StartStrongAttack()
        {
            weaponHitbox.damageType = DamageType.Strong;
            weaponHitbox.baseDamage = 25f;
        }

        // 궁극기
        public void StartUltimateAttack()
        {
            weaponHitbox.damageType = DamageType.Ultimate;
            weaponHitbox.baseDamage = 100f;
        }

        // 실질적 타격 타이밍에서 호출될 함수들
        public void EnableHitbox()
        {
            weaponHitbox.EnableHitbox();
        }

        public void DisableHitbox()
        {
            weaponHitbox.DisableHitbox();
        }
        #endregion
    }
}
