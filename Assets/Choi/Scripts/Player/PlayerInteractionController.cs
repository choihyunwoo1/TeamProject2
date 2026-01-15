using UnityEngine;
using UnityEngine.InputSystem;

namespace Choi
{
    public class PlayerInteractionController : MonoBehaviour
    {
        [Header("Interaction Settings")]
        [SerializeField] private float interactRange = 3f;
        [SerializeField] private LayerMask interactLayer;

        private Camera _cam;
        private IInteractable currentInteractable;

        private void Awake()
        {
            _cam = Camera.main;
        }

        private void Update()
        {
            DetectInteractable();
        }

        // Input System에서 "Interact" 액션이 호출됨
        public void OnInteract(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            TryInteract();
        }

        private void DetectInteractable()
        {
            currentInteractable = null;

            Ray ray = new Ray(_cam.transform.position, _cam.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactLayer))
            {
                currentInteractable = hit.collider.GetComponent<IInteractable>();

                if (currentInteractable != null)
                {
                    InteractionUI.Instance.Show(currentInteractable.GetInteractPrompt());
                    return;
                }
            }

            InteractionUI.Instance.Hide();
        }

        private void TryInteract()
        {
            if (currentInteractable == null) return;

            currentInteractable.Interact(gameObject);
        }
    }
}
