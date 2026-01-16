using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class DialoguePanelController : MonoBehaviour
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

    private NPCDialogue currentNPC;

    private void Start()
    {
        panel.SetActive(false); // Start 시 panel만 꺼짐
    }

    private void Update()
    {
        if (!panel.activeSelf) return;

        // E키 → 다음 대사
        if (Keyboard.current.eKey.wasPressedThisFrame)
            OnNext();

        // 이동키 → 대화 종료
        if (Keyboard.current.wKey.isPressed || Keyboard.current.aKey.isPressed ||
            Keyboard.current.sKey.isPressed || Keyboard.current.dKey.isPressed ||
            Keyboard.current.upArrowKey.isPressed || Keyboard.current.downArrowKey.isPressed ||
            Keyboard.current.leftArrowKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            Hide();
        }

        // 스페이스 또는 마우스 클릭 → 대화 종료
        if (Keyboard.current.spaceKey.wasPressedThisFrame ||
            Mouse.current.leftButton.wasPressedThisFrame ||
            Mouse.current.rightButton.wasPressedThisFrame)
        {
            Hide();
        }
    }

    public void Show(string speaker, string line, NPCDialogue npc)
    {
        currentNPC = npc;
        panel.SetActive(true);
        nameText.text = speaker;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeLine(line));
    }

    private IEnumerator TypeLine(string line)
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
}
