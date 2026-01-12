using UnityEngine;
using UnityEngine.InputSystem;

namespace Choi
{
    public class PlayerCameraController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform target;          // 플레이어
        [SerializeField] private Transform cameraPivot;     // 회전 중심
        [SerializeField] private Camera mainCamera;

        [Header("Rotation Settings")]
        [SerializeField] private float lookSensitivity = 0.6f;    // 기존 1.5f → 대폭 낮춰서 부드럽게
        [SerializeField] private float minPitch = -30f;
        [SerializeField] private float maxPitch = 70f;

        [Header("Follow Settings")]
        [SerializeField] private float followSpeed = 10f;   // 부드러운 추적 속도
        [SerializeField] private Vector3 cameraOffset = new Vector3(0, 2f, -3.5f);

        private Vector2 lookInput;
        private float yaw;
        private float pitch;

        public void OnLook(InputAction.CallbackContext context)
        {
            lookInput = context.ReadValue<Vector2>();
        }

        private void Start()
        {
            if (mainCamera == null)
                mainCamera = Camera.main;

            Vector3 angles = cameraPivot.eulerAngles;
            yaw = angles.y;
            pitch = angles.x;
        }

        private void LateUpdate()
        {
            RotateCamera();
            FollowTarget();
        }

        private void RotateCamera()
        {
            // 회전 입력
            yaw += lookInput.x * lookSensitivity;
            pitch -= lookInput.y * lookSensitivity;

            // 상하 제한
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            // 회전 적용
            cameraPivot.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }

        private void FollowTarget()
        {
            // 카메라 리그가 플레이어를 스무스하게 따라오도록 설정
            Vector3 targetPos = target.position;

            transform.position = Vector3.Lerp(
                transform.position,
                targetPos,
                Time.deltaTime * followSpeed
            );

            // 카메라 위치도 Pivot 기준으로 보정
            mainCamera.transform.localPosition = cameraOffset;
            mainCamera.transform.LookAt(cameraPivot);
        }
    }
}
