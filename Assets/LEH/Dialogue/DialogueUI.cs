using TMPro;
using UnityEngine;
using System.Collections;

public class DialogueUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject panel;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text dialogueText;

    [Header("Setting")]
    [SerializeField] float typingSpeed = 0.04f;

    Coroutine typingCoroutine;
    bool isTyping;
    string fullText;

    NPCDialogue currentNPC;

    void Start()
    {
        panel.SetActive(false); //시작할 때 숨김
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
        // 타이핑 중이면 즉시 출력
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = fullText;
            isTyping = false;
        }
        // 다 출력된 상태면 다음 대사
        else
        {
            currentNPC.NextLine();
        }
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}
