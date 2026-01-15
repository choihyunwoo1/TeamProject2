using UnityEngine;

namespace Choi
{
    public class DoorInteractable : MonoBehaviour, IInteractable
    {
        public Animator animator;

        public string GetInteractPrompt()
        {
            return "Open : [E]";
        }

        public void Interact(GameObject player)
        {
            animator.SetTrigger("Open");
        }
    }
}