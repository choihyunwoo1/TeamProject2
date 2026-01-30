using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject tutorialRoot;
    public Image dimBackground;
    public RectTransform explanationPanel;
    public Text descriptionText;
    public Button nextButton;
    public Button exitButton;

    [Header("Data")]
    public TutorialData currentData;

    int stepIndex = 0;

    void Awake()
    {
        tutorialRoot.SetActive(false);
    }

    public void StartTutorial(TutorialData data)
    {
        currentData = data;
        stepIndex = 0;

        tutorialRoot.SetActive(true);
        dimBackground.gameObject.SetActive(true);

        ShowStep();
    }

    void ShowStep()
    {
        TutorialStep step = currentData.steps[stepIndex];

        // 텍스트
        descriptionText.text = step.description;

        // 위치 & 크기
        explanationPanel.anchoredPosition = step.anchoredPosition;
        explanationPanel.sizeDelta = step.size;

        // 버튼 리스너 초기화
        nextButton.onClick.RemoveAllListeners();
        exitButton.onClick.RemoveAllListeners();

        bool isLast = stepIndex == currentData.steps.Length - 1;

        nextButton.gameObject.SetActive(!isLast);
        exitButton.gameObject.SetActive(isLast);

        if (isLast)
            exitButton.onClick.AddListener(EndTutorial);
        else
            nextButton.onClick.AddListener(NextStep);
    }

    void NextStep()
    {
        stepIndex++;
        ShowStep();
    }

    void EndTutorial()
    {
        tutorialRoot.SetActive(false);
    }
}
