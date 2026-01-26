using UnityEngine;
using UnityEngine.InputSystem;

namespace Choi
{
    public class PlayerCameraController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform target;
        [SerializeField] private Transform cameraPivot;
        [SerializeField] private Camera mainCamera;

        [Header("Rotation Settings")]
        [SerializeField] private float lookSensitivity = 0.6f;
        [SerializeField] private float minPitch = -30f;
        [SerializeField] private float maxPitch = 70f;
        [SerializeField] private float rotationSmoothTime = 0.1f;

        [Header("Follow Settings")]
        [SerializeField] private float followSpeed = 10f;
        [SerializeField] private Vector3 cameraOffset = new Vector3(0, 2f, -3.5f);

        [Header("Collision Settings")]
        [SerializeField] private float collisionRadius = 0.2f;
        [SerializeField] private float collisionSmooth = 0.05f;
        [SerializeField] private LayerMask collisionMask;

        private Vector2 lookInput;
        private float yaw;
        private float pitch;

        private float smoothYaw;
        private float smoothPitch;
        private float yawVelocity;
        private float pitchVelocity;

        private Vector3 cameraVelocity;
        private float currentZOffset;  // 충돌 시 Z 보정

        public void OnLook(InputAction.CallbackContext context)
        {
            if (float.IsNaN(lookInput.x) || float.IsNaN(lookInput.y))
                lookInput = Vector2.zero;

            lookInput = context.ReadValue<Vector2>();
        }

        private void Start()
        {
            if (mainCamera == null)
                mainCamera = Camera.main;

            currentZOffset = cameraOffset.z;

            Vector3 angles = cameraPivot.eulerAngles;
            yaw = smoothYaw = angles.y;
            pitch = smoothPitch = angles.x;
        }

        private void LateUpdate()
        {
            RotateCamera();
            FollowTarget();
            HandleCameraCollision();
        }
        private void RotateCamera()
        {
            yaw += lookInput.x * lookSensitivity;
            pitch -= lookInput.y * lookSensitivity;

            // pitch 제한
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            // yaw 무한 증가 방지
            yaw = Mathf.Repeat(yaw, 360f);

            smoothYaw = Mathf.SmoothDampAngle(smoothYaw, yaw, ref yawVelocity, rotationSmoothTime);
            smoothPitch = Mathf.SmoothDampAngle(smoothPitch, pitch, ref pitchVelocity, rotationSmoothTime);

            cameraPivot.rotation = Quaternion.Euler(smoothPitch, smoothYaw, 0f);
        }


        private void FollowTarget()
        {
            transform.position = Vector3.Lerp(
                transform.position,
                target.position,
                Time.deltaTime * followSpeed
            );
        }

        private void HandleCameraCollision()
        {
            // 원래 카메라가 위치해야 하는 곳
            Vector3 desiredCameraPos =
                cameraPivot.position
                + cameraPivot.right * cameraOffset.x
                + cameraPivot.up * cameraOffset.y
                + cameraPivot.forward * currentZOffset;

            Vector3 direction = desiredCameraPos - cameraPivot.position;

            float distance = Mathf.Abs(currentZOffset);

            // SphereCast로 카메라 충돌 체크
            if (Physics.SphereCast(
                    cameraPivot.position,
                    collisionRadius,
                    direction.normalized,
                    out RaycastHit hit,
                    distance,
                    collisionMask))
            {
                float hitDist = hit.distance - 0.1f;

                // 벽과 너무 붙지 않게 0.1f만큼 여유
                float targetZ = -hitDist;

                currentZOffset = Mathf.Lerp(currentZOffset, targetZ, Time.deltaTime * 10f);
            }
            else
            {
                // 아무것도 안 맞으면 원래 거리로 회복
                currentZOffset = Mathf.Lerp(currentZOffset, cameraOffset.z, Time.deltaTime * 5f);
            }

            // 실제 카메라 이동
            Vector3 finalPos =
                cameraPivot.position
                + cameraPivot.right * cameraOffset.x
                + cameraPivot.up * cameraOffset.y
                + cameraPivot.forward * currentZOffset;

            mainCamera.transform.position =
                Vector3.SmoothDamp(mainCamera.transform.position, finalPos, ref cameraVelocity, collisionSmooth);

            mainCamera.transform.LookAt(cameraPivot);
        }
    }
}
