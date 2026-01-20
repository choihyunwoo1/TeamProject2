using UnityEngine;

public class NPCDialogueNew : MonoBehaviour
{
    [Header("NPC Info")]
    public string npcName;
    [TextArea] public string[] lines;

    [Header("References")]
    [SerializeField] private DialogueUINew dialogueUI;

    private int index;
    private bool isTalking;

    private void Start()
    {
        if (dialogueUI == null)
            dialogueUI = FindFirstObjectByType<DialogueUINew>();
    }

    public void StartDialogue()
    {
        if (isTalking) return;

        index = 0;
        isTalking = true;

        dialogueUI.Show(npcName, lines[index], this);
    }

    public void NextLine()
    {
        if (!isTalking) return;

        index++;

        if (index < lines.Length)
            dialogueUI.Show(npcName, lines[index], this);
        else
            EndDialogue();
    }

    void EndDialogue()
    {
        isTalking = false;
        dialogueUI.Hide();
    }
}

