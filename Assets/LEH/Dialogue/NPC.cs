using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class NPC : MonoBehaviour
{
    [Header("NPC Info")]
    public string npcName;
    [TextArea] public string[] lines;

    [Header("References")]
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private Transform player;

    [Header("Layer Settings")]
    [SerializeField] private LayerMask npcLayer;

    private int index;

    void Start()
    {
        if (dialogueUI == null)
            dialogueUI = FindFirstObjectByType<DialogueUI>();
    }

    void Update()
    {
        if (Mouse.current == null) return;
        if (!Mouse.current.leftButton.wasPressedThisFrame) return;

        // UI 위 클릭이면 무시
        if (EventSystem.current.IsPointerOverGameObject()) return;

        Vector3 rayOrigin = player.position + Vector3.up * 1.5f;
        Vector3 rayDir = (transform.position - rayOrigin).normalized;

        if (Physics.Raycast(rayOrigin, rayDir, out RaycastHit hit, 10f, npcLayer))
        {
            if (hit.transform == transform)
            {
                Debug.Log("NPC 클릭 감지 성공!");
                StartDialogue();
            }
        }
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
            dialogueUI.Show(npcName, lines[index], this);
        else
            dialogueUI.Hide();
    }
}
