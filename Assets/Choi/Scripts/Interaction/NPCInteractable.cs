using UnityEngine;

namespace Choi
{
    public class NPCInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private string npcName = "NPC";
        [SerializeField] private TextAsset dialogueJSON;

        public string GetInteractPrompt() => "대화하기";

        public void Interact(GameObject player)
        {
            DialogueManager.Instance.StartDialogue(npcName, dialogueJSON);
        }
    }
}