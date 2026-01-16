using UnityEngine;
using UnityEngine.InputSystem;

namespace Choi
{
    public class PlayerAttackDialogueHook : MonoBehaviour
    {
        [Header("Ray Settings")]
        [SerializeField] private float rayDistance = 10f;
        [SerializeField] private LayerMask npcLayer;

        private Camera cam;

        private void Start()
        {
            // MainCamera 자동 참조
            cam = GetComponentInChildren<Camera>();
            if (cam == null)
                Debug.LogError("Camera를 찾을 수 없습니다! Player 자식에 MainCamera 있어야 함");
        }

        private void Update()
        {
            if (Mouse.current == null) return;

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Debug.Log("공격 입력 감지");
                FireRayFromCenter();
            }
        }

        private void FireRayFromCenter()
        {
            if (cam == null) return;

            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red, 1f);

            if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, npcLayer))
            {
                Debug.Log("맞은 오브젝트: " + hit.collider.name);
                var npc = hit.collider.GetComponent<NPCDialogueNew>();
                if (npc != null)
                {
                    npc.StartDialogue();
                }
            }
            else
            {
                Debug.Log("❌ 아무것도 안 맞음");
            }
        }
    }
}
