using UnityEngine;

namespace Choi
{
    public class BookInteractable : MonoBehaviour, IInteractable
    {
        public string GetInteractPrompt()
        {
            return "Read : [E]";
        }

        public void Interact(GameObject player)
        {

        }
    }
}