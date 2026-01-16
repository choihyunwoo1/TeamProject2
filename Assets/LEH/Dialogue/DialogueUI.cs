using TMPro;
using UnityEngine;
using System.Collections;

public class DialogueUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject panel;       // 대화창 패널
    [SerializeField] private TMP_Text nameText;      // 화자 이름
    [SerializeField] private TMP_Text dialogueText;  // 대사 텍스트

    [Header("Settings")]
    [SerializeField] private float typingSpeed = 0.04f;

    private Coroutine typingCoroutine;
    private bool isTyping;
    private string fullText;
    private NPC currentNPC;

    void Start()
    {
        if (panel == null)
            Debug.LogError("DialogueUI: panel이 연결되어 있지 않습니다!");
        panel.SetActive(false);
    }

    public void Show(string speaker, string line, NPC npc)
    {
        currentNPC = npc;

        Debug.Log("DialogueUI.Show 호출됨: " + speaker + " / " + line);

        if (panel == null) return;
        panel.SetActive(true);

        if (nameText != null) nameText.text = speaker;
        else Debug.LogError("nameText가 null!");

        if (dialogueText != null) dialogueText.text = "";
        else Debug.LogError("dialogueText가 null!");

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
            if (currentNPC != null)
                currentNPC.NextLine();
        }
    }

    public void Hide()
    {
        if (panel != null)
            panel.SetActive(false);
    }
}
