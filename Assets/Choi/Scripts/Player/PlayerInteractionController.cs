using UnityEngine;
using UnityEngine.InputSystem;

namespace Choi
{
    public class PlayerInteractionController : MonoBehaviour
    {
        [Header("Interaction Settings")]
        [SerializeField] private float interactRange = 3f;
        [SerializeField] private LayerMask interactLayer;

        [Header("Player Root")]
        [SerializeField] private Transform playerRoot;
        [SerializeField] private float heightOffset = 1.2f;

        [Header("BoxCast Settings")]
        [SerializeField] private Vector3 boxHalfExtents = new Vector3(10.0f, 10.0f, 10.0f);

        private IInteractable currentInteractable;

        private void Update()
        {
            DetectInteractable();
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            TryInteract();
        }

        private void DetectInteractable()
        {
            currentInteractable = null;

            // 시작 위치: 플레이어 기준 약간 위
            Vector3 origin = playerRoot.position + Vector3.up * heightOffset;

            // BoxCast 방향
            Vector3 direction = playerRoot.forward;

            // 디버그용
            Debug.DrawRay(origin, direction * interactRange, Color.yellow);

            if (Physics.BoxCast(
                origin,
                boxHalfExtents,
                direction,
                out RaycastHit hit,
                playerRoot.rotation,
                interactRange,
                interactLayer))
            {
                currentInteractable = hit.collider.GetComponentInParent<IInteractable>();

                if (currentInteractable != null)
                {
                    InteractionUI.Instance.Show(currentInteractable.GetInteractPrompt());
                    return;
                }
            }

            InteractionUI.Instance.Hide();
        }
        private void OnDrawGizmos()
        {
            if (playerRoot == null) return;

            Gizmos.color = Color.cyan;

            Vector3 origin = playerRoot.position + Vector3.up * heightOffset;
            Gizmos.matrix = Matrix4x4.TRS(origin, playerRoot.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.forward * (interactRange * 0.5f),
                                boxHalfExtents * 2);
        }

        private void TryInteract()
        {
            if (currentInteractable == null) return;
            currentInteractable.Interact(gameObject);
        }
    }
}
