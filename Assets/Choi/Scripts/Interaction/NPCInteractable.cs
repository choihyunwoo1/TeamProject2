using UnityEngine;

namespace Choi
{
    public class NPCInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private TextAsset dialogueJSON;

        public string GetInteractPrompt()
        {
            return "Chat : [E]";
        }

        public void Interact(GameObject player)
        {
            DialogueManager.Instance.StartDialogue(dialogueJSON);
        }
    }
}