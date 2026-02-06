using Choi;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public PlayerInput playerInput;
    private PlayerStats playerStats;

    [Header("UI")]
    public GameObject tutorialRoot;
    public RectTransform explanationPanel;
    public RectTransform descriptionRect;
    public TMP_Text descriptionText;
    public Button nextButton;
    public Button exitButton;

    [Header("Data")]
    public TutorialData currentData;

    int stepIndex = 0;

    void Awake()
    {
        tutorialRoot.SetActive(false);
    }

    private void Start()
    {
        // 플레이어 생성 타이밍과 상관없이 검색
        StartCoroutine(WaitForPlayer());
    }
    private IEnumerator WaitForPlayer()
    {
        while (playerStats == null || playerInput == null)
        {
            if (playerStats == null)
                playerStats = FindFirstObjectByType<PlayerStats>();

            if (playerInput == null)
                playerInput = FindFirstObjectByType<PlayerInput>();

            yield return null;
        }

        Debug.Log("TutorialManager: PlayerStats & PlayerInput 연결 완료");
    }

    public void StartTutorial(TutorialData data)
    {
        if (data == null || data.steps.Length == 0)
            return;

        currentData = data;
        stepIndex = 0;

        if (playerInput != null)
            playerInput.enabled = false;

        tutorialRoot.SetActive(true);
        ShowStep();
    }


    void ShowStep()
    {
        TutorialStep step = currentData.steps[stepIndex];

        // 텍스트
        descriptionText.text = step.description;

        // Anchor
        ApplyAnchor(explanationPanel, step.anchorType);

        // Panel
        explanationPanel.anchoredPosition = step.panelOffset;
        explanationPanel.sizeDelta = step.panelSize;

        // Text Rect
        descriptionRect.anchoredPosition = step.textOffset;
        descriptionRect.sizeDelta = step.textSize;

        // 버튼 초기화
        nextButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);

        nextButton.onClick.RemoveAllListeners();
        exitButton.onClick.RemoveAllListeners();

        // 버튼 타입 처리
        switch (step.buttonType)
        {
            case TutorialButtonType.Next:
                nextButton.gameObject.SetActive(true);
                nextButton.onClick.AddListener(NextStep);
                break;

            case TutorialButtonType.Exit:
                exitButton.gameObject.SetActive(true);
                exitButton.onClick.AddListener(EndTutorial);
                break;

            case TutorialButtonType.None:
                break;
        }
    }

    void NextStep()
    {
        stepIndex++;

        if (stepIndex >= currentData.steps.Length)
        {
            EndTutorial();
            return;
        }

        ShowStep();
    }
    void EndTutorial()
    {
        PlayerPrefs.SetInt($"Tutorial_{currentData.tutorialId}", 1);
        PlayerPrefs.Save();

        if (playerInput != null)
            playerInput.enabled = true;

        tutorialRoot.SetActive(false);
    }

    void ApplyAnchor(RectTransform rt, TutorialAnchor type)
    {
        Vector2 anchor = Vector2.zero;

        switch (type)
        {
            case TutorialAnchor.LeftTop: anchor = new Vector2(0f, 1f); break;
            case TutorialAnchor.Top: anchor = new Vector2(0.5f, 1f); break;
            case TutorialAnchor.RightTop: anchor = new Vector2(1f, 1f); break;

            case TutorialAnchor.LeftCenter: anchor = new Vector2(0f, 0.5f); break;
            case TutorialAnchor.Center: anchor = new Vector2(0.5f, 0.5f); break;
            case TutorialAnchor.RightCenter: anchor = new Vector2(1f, 0.5f); break;

            case TutorialAnchor.LeftBottom: anchor = new Vector2(0f, 0f); break;
            case TutorialAnchor.Bottom: anchor = new Vector2(0.5f, 0f); break;
            case TutorialAnchor.RightBottom: anchor = new Vector2(1f, 0f); break;
        }

        rt.anchorMin = rt.anchorMax = anchor;
        rt.pivot = new Vector2(0.5f, 0.5f);
    }
}
