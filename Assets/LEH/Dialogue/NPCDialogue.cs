using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [Header("NPC Info")]
    public string npcName;
    [TextArea] public string[] lines;

    [Header("References")]
    public Transform player;
    public DialoguePanelController dialogueUI;

    [Header("Settings")]
    public float talkDistance = 3f;

    private int index;

    private void OnMouseDown()
    {
        // 플레이어와 NPC 거리 체크
        float dist = Vector3.Distance(player.position, transform.position);
        if (dist > talkDistance)
        {
            Debug.Log("NPC 너무 멀어서 대화 불가");
            return;
        }

        Debug.Log("NPC 클릭 감지 성공!");
        StartDialogue();
    }

    public void StartDialogue()
    {
        index = 0;
        dialogueUI.Show(npcName, lines[index], this);
    }

    public void NextLine()
    {
        index++;
        if (index < lines.Length)
            dialogueUI.Show(npcName, lines[index], this);
        else
            dialogueUI.Hide();
    }
}

