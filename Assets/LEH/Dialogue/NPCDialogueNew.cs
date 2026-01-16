using UnityEngine;
using UnityEngine.InputSystem;

public class NPCDialogueNew : MonoBehaviour
{
    [Header("NPC Info")]
    public string npcName;
    [TextArea] public string[] lines;

    [Header("References")]
    [SerializeField] private DialogueUINew dialogueUI;
    [SerializeField] private Transform player;

    [Header("Settings")]
    [SerializeField] private float talkDistance = 3f;
    [SerializeField] private LayerMask npcLayer;

    private int index;
    private bool isTalking;

    void Start()
    {
        if (dialogueUI == null)
            dialogueUI = FindFirstObjectByType<DialogueUINew>();
    }

    void Update()
    {
        if (Mouse.current == null) return;
        if (!Mouse.current.leftButton.wasPressedThisFrame) return;

        TryClickNPC();
    }

    void TryClickNPC()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        Debug.DrawRay(ray.origin, ray.direction * 20f, Color.green, 1f);

        // LayerMask 적용
        if (!Physics.Raycast(ray, out RaycastHit hit, 100f, npcLayer))
        {
            Debug.Log("Ray 아무것도 안 맞음");
            return;
        }

        Debug.Log("맞은 오브젝트: " + hit.transform.name);

        if (hit.transform != transform) return;

        float dist = Vector3.Distance(player.position, transform.position);
        if (dist > talkDistance)
        {
            Debug.Log("너무 멀어서 대화 불가");
            return;
        }

        StartDialogue();
    }

    public void StartDialogue()
    {
        index = 0;
        isTalking = true;

        dialogueUI.Show(npcName, lines[index], this);
    }

    public void NextLine()
    {
        if (!isTalking) return;

        index++;
        if (index < lines.Length)
        {
            dialogueUI.Show(npcName, lines[index], this);
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        isTalking = false;
        dialogueUI.Hide();
    }
}
