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
        [SerializeField] private float rotationSmoothTime = 0.05f;

        [Header("Follow Settings")]
        [SerializeField] private float followSpeed = 10f;
        [SerializeField] private Vector3 cameraOffset = new Vector3(0, 2f, -3.5f);

        private Vector2 lookInput;
        private float yaw;
        private float pitch;

        private float smoothYaw;
        private float smoothPitch;
        private float yawVelocity;
        private float pitchVelocity;

        private Vector3 cameraVelocity;

        public void OnLook(InputAction.CallbackContext context)
        {
            lookInput = context.ReadValue<Vector2>();
        }

        private void Start()
        {
            if (mainCamera == null)
                mainCamera = Camera.main;

            Vector3 angles = cameraPivot.eulerAngles;
            yaw = smoothYaw = angles.y;
            pitch = smoothPitch = angles.x;
        }

        private void LateUpdate()
        {
            RotateCamera();
            FollowTarget();
        }

        private void RotateCamera()
        {
            // 입력 기반 회전
            yaw += lookInput.x * lookSensitivity;
            pitch -= lookInput.y * lookSensitivity;

            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            // 부드러운 회전 보간
            smoothYaw = Mathf.SmoothDampAngle(smoothYaw, yaw, ref yawVelocity, rotationSmoothTime);
            smoothPitch = Mathf.SmoothDampAngle(smoothPitch, pitch, ref pitchVelocity, rotationSmoothTime);

            cameraPivot.rotation = Quaternion.Euler(smoothPitch, smoothYaw, 0f);
        }

        private void FollowTarget()
        {
            // Rig가 플레이어를 따라감
            transform.position = Vector3.Lerp(
                transform.position,
                target.position,
                Time.deltaTime * followSpeed
            );

            // Pivot 기준 카메라 위치 계산
            Vector3 desiredCameraPos =
                cameraPivot.position
                + cameraPivot.right * cameraOffset.x
                + cameraPivot.up * cameraOffset.y
                + cameraPivot.forward * cameraOffset.z;

            // 부드러운 카메라 이동
            mainCamera.transform.position =
                Vector3.SmoothDamp(mainCamera.transform.position, desiredCameraPos, ref cameraVelocity, 0.05f);

            mainCamera.transform.LookAt(cameraPivot);
        }
    }
}
