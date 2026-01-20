using UnityEngine;
using UnityEngine.InputSystem;

    public class PlayerAttackDialogueHook : MonoBehaviour
    {
        [Header("Ray Settings")]
        [SerializeField] private float rayDistance = 50f;
        [SerializeField] private LayerMask npcLayer;
        [SerializeField] private float sphereRadius = 0.6f;

        private Camera cam;

        private void Start()
        {
            cam = GetComponentInChildren<Camera>();
        }

        private void Update()
        {
            if (Mouse.current == null) return;
            if (!Mouse.current.leftButton.wasPressedThisFrame) return;

            // 1. NPC 먼저 체크
            if (TryNPC())
                return;

            // 2. NPC 없으면 공격
            DoAttack();
        }

        bool TryNPC()
        {
            Ray ray = cam.ScreenPointToRay(
                Mouse.current.position.ReadValue());

            Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red, 1f);

            if (Physics.SphereCast(
                ray.origin,
                sphereRadius,
                ray.direction,
                out RaycastHit hit,
                rayDistance,
                npcLayer))
            {
                Debug.Log("NPC 감지: " + hit.collider.name);

                hit.collider
                    .GetComponent<NPCDialogueNew>()
                    ?.StartDialogue();

                return true;
            }

            return false;
        }

        void DoAttack()
        {
            Debug.Log("공격 실행");
            // 기존 공격 코드
        }
    }
