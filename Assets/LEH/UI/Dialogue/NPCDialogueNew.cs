using UnityEngine;

public class NPCDialogueNew : MonoBehaviour
{
    [Header("NPC Info")]
    public string npcName;
    [TextArea] public string[] lines;

    [Header("Choices")]
    [TextArea] public string[] choices;

    [Header("Choice Timing")]
    public bool useChoice;
    public int choiceIndex; // 몇 번째 줄에서 선택지 뜰지

    [Header("References")]
    [SerializeField] private DialogueUINew dialogueUI;

    private int index;
    private bool isTalking;
    private bool choiceUsed;

    void Start()
    {
        if (dialogueUI == null)
            dialogueUI = FindFirstObjectByType<DialogueUINew>();
    }

    public void StartDialogue()
    {
        if (isTalking) return;

        index = 0;
        isTalking = true;
        choiceUsed = false;

        ShowLine();
    }

    void ShowLine()
    {
        if (index >= lines.Length)
        {
            EndDialogue();
            return;
        }

        bool isLast = index == lines.Length - 1;

        dialogueUI.Show(
            npcName,
            lines[index],
            this,
            isLast
        );

        // 선택지 타이밍
        if (useChoice && !choiceUsed && index == choiceIndex)
        {
            choiceUsed = true;
            dialogueUI.ShowChoices(choices);
        }
    }

    public void NextLine()
    {
        index++;
        ShowLine();
    }

    // UI에서 호출
    public void OnChoiceSelected(int id)
    {
        Debug.Log("선택한 번호: " + id);

        NextLine();
    }

    public bool IsLastLine()
    {
        return index >= lines.Length - 1;
    }

    public void EndDialogue()
    {
        isTalking = false;
        dialogueUI.Hide();
    }
}
