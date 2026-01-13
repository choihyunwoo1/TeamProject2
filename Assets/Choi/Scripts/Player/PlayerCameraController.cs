using UnityEngine;
using UnityEngine.InputSystem;

namespace Choi
{
    public class PlayerCameraController : MonoBehaviour
    {
        #region Variables
        [Header("References")]
        [SerializeField] private Transform target;          // 플레이어
        [SerializeField] private Transform cameraPivot;     // 회전 중심 (CameraRig의 자식)
        [SerializeField] private Camera mainCamera;

        [Header("Rotation Settings")]
        [SerializeField] private float lookSensitivity = 0.6f;
        [SerializeField] private float minPitch = -30f;
        [SerializeField] private float maxPitch = 70f;

        [Header("Follow Settings")]
        [SerializeField] private float followSpeed = 10f;
        [SerializeField] private Vector3 cameraOffset = new Vector3(0, 2f, -3.5f);

        private Vector2 lookInput;
        private float yaw;
        private float pitch;
        #endregion

        #region Unity Event Method
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
        #endregion

        #region Custom Method
        private void RotateCamera()
        {
            // 입력 기반 회전 적용
            yaw += lookInput.x * lookSensitivity;
            pitch -= lookInput.y * lookSensitivity;

            // 피치 제한
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            // 회전 적용 (Pivot 기준)
            cameraPivot.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }

        private void FollowTarget()
        {
            // 1) Rig 자체가 플레이어를 따라가도록 이동 (부드럽게)
            transform.position = Vector3.Lerp(
                transform.position,
                target.position,
                Time.deltaTime * followSpeed
            );

            // 2) Camera 위치를 Pivot 기준으로 로컬 오프셋 적용
            Vector3 desiredCameraPos =
                cameraPivot.position
                + cameraPivot.right * cameraOffset.x
                + cameraPivot.up * cameraOffset.y
                + cameraPivot.forward * cameraOffset.z;

            mainCamera.transform.position = desiredCameraPos;

            // 3) 카메라는 항상 Pivot을 바라본다
            mainCamera.transform.LookAt(cameraPivot);
        }
        #endregion
    }
}
