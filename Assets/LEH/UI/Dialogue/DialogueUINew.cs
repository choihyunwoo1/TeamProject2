using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class DialogueUINew : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text dialogueText;

    [Header("Settings")]
    [SerializeField] private float typingSpeed = 0.04f;

    private Coroutine typingCoroutine;
    private bool isTyping;
    private string fullText;
    private NPCDialogueNew currentNPC;

    void Start()
    {
        panel.SetActive(false);
    }

    public void Show(string speaker, string line, NPCDialogueNew npc)
    {
        currentNPC = npc;
        panel.SetActive(true);
        nameText.text = speaker;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeLine(line));
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        fullText = line;
        dialogueText.text = "";

        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    public void OnNext()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = fullText;
            isTyping = false;
        }
        else
        {
            currentNPC?.NextLine();
        }
    }

    public void Hide()
    {
        panel.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            OnNext();
        }
    }
}
