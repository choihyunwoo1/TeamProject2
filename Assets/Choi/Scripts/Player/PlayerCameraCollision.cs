using UnityEngine;

namespace Choi
{
    public class PlayerCameraCollision : MonoBehaviour
    {
        #region Variables
        [Header("References")]
        [SerializeField] private Transform cameraPivot;   // CameraRig (회전축)
        [SerializeField] private Transform cameraTransform; // Main Camera

        [Header("Settings")]
        [SerializeField] private float collisionRadius = 0.2f;
        [SerializeField] private float collisionOffset = 0.2f;
        [SerializeField] private float cameraSmooth = 10f;

        private float defaultDistance;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            // 초기 카메라 거리 저장 (offset 기준)
            defaultDistance = Vector3.Distance(cameraPivot.position, cameraTransform.position);
        }

        private void LateUpdate()
        {
            HandleCollision();
        }
        #endregion

        #region Custom Method
        private void HandleCollision()
        {
            Vector3 pivotPos = cameraPivot.position;
            Vector3 dir = (cameraTransform.position - pivotPos).normalized;

            float desiredDistance = defaultDistance;

            // 충돌 체크
            if (Physics.SphereCast(
                pivotPos,
                collisionRadius,
                dir,
                out RaycastHit hit,
                defaultDistance + collisionOffset
            ))
            {
                // 충돌 지점까지의 최소 거리 계산
                desiredDistance = hit.distance - collisionOffset;
            }

            // 음수가 되면 안됨
            desiredDistance = Mathf.Clamp(desiredDistance, 0.3f, defaultDistance);

            // 카메라 위치 보정 (스무스하게 이동)
            Vector3 targetPos = pivotPos + dir * desiredDistance;

            cameraTransform.position = Vector3.Lerp(
                cameraTransform.position,
                targetPos,
                Time.deltaTime * cameraSmooth
            );
        }
        #endregion
    }
}
