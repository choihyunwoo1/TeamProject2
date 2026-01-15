using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [Header("NPC Info")]
    public string npcName;

    [TextArea]
    public string[] lines;

    [Header("Reference")]
    [SerializeField] DialogueUI dialogueUI; // 직접 연결

    int index;

    void OnMouseDown()
    {
        StartDialogue();
    }

    void StartDialogue()
    {
        index = 0;
        dialogueUI.Show(npcName, lines[index], this);
    }

    public void NextLine()
    {
        index++;

        if (index < lines.Length)
        {
            dialogueUI.Show(npcName, lines[index], this);
        }
        else
        {
            dialogueUI.Hide();
        }
    }
}
