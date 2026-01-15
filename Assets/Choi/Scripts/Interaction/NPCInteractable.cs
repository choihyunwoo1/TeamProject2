using UnityEngine;

namespace Choi
{
    public class NPCInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private TextAsset dialogueJSON;

        public string GetInteractPrompt()
        {
            return "Press [E] Button";
        }

        public void Interact(GameObject player)
        {
            DialogueManager.Instance.StartDialogue(dialogueJSON);
        }
    }
}