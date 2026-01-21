using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueUINew : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Button nextButton;
    [SerializeField] private TMP_Text nextButtonText;

    [Header("Choice UI")]
    [SerializeField] private GameObject choiceGroup;
    [SerializeField] private Button[] choiceButtons;

    [Header("Settings")]
    [SerializeField] private float typingSpeed = 0.04f;

    private Coroutine typingCoroutine;
    private bool isTyping;
    private string fullText;
    private NPCDialogueNew currentNPC;

    void Start()
    {
        panel.SetActive(false);
        choiceGroup.SetActive(false);
    }

    public void Show(string speaker, string line, NPCDialogueNew npc, bool isLast)
    {
        currentNPC = npc;

        panel.SetActive(true);
        nameText.text = speaker;

        nextButtonText.text =
            isLast ? "클릭해서 닫기 ▼" : "클릭해서 넘기기 ▼";

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

    // Next 버튼
    public void OnNext()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = fullText;
            isTyping = false;
            return;
        }

        if (currentNPC.IsLastLine())
            currentNPC.EndDialogue();
        else
            currentNPC.NextLine();
    }

    // ================= 선택지 =================

    public void ShowChoices(string[] choices)
    {
        nextButton.gameObject.SetActive(false);
        choiceGroup.SetActive(true);

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            int index = i;

            choiceButtons[i]
                .GetComponentInChildren<TMP_Text>()
                .text = choices[i];

            choiceButtons[i].onClick.RemoveAllListeners();
            choiceButtons[i].onClick.AddListener(() =>
            {
                OnChoiceSelected(index);
            });
        }
    }

    void OnChoiceSelected(int index)
    {
        StartCoroutine(ChoiceEffect(index));
        currentNPC.OnChoiceSelected(index);
    }

    IEnumerator ChoiceEffect(int index)
    {
        Button btn = choiceButtons[index];
        Outline outline = btn.GetComponent<Outline>();

        if (outline != null)
            outline.enabled = true;

        Vector3 origin = btn.transform.localScale;
        btn.transform.localScale = origin * 1.1f;

        yield return new WaitForSeconds(0.15f);

        btn.transform.localScale = origin;

        yield return new WaitForSeconds(0.4f);

        if (outline != null)
            outline.enabled = false;

        choiceGroup.SetActive(false);
        nextButton.gameObject.SetActive(true);
    }

    // ================= 종료 =================

    public void Hide()
    {
        panel.SetActive(false);
        choiceGroup.SetActive(false);
    }
}
