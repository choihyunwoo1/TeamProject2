using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [Header("Tutorial Data")]
    public TutorialData tutorialData;

    TutorialManager tutorialManager;
    bool triggered = false;

    void Awake()
    {
        tutorialManager = Object.FindFirstObjectByType<TutorialManager>();

        if (tutorialManager == null)
            Debug.LogError("TutorialManager를 씬에서 찾을 수 없음");
    }

    void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;
        if (tutorialData == null) return;

        // 이미 본 튜토리얼이면 실행 안 함
       /* if (PlayerPrefs.GetInt($"Tutorial_{tutorialData.tutorialId}", 0) == 1)
            return;*/

        triggered = true;
        tutorialManager.StartTutorial(tutorialData);
    }
}
