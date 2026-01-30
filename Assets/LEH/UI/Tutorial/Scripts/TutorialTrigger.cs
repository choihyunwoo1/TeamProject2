using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public TutorialData tutorialData;      // 실행할 튜토리얼 데이터
    public TutorialManager tutorialManager;

    bool triggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            tutorialManager.StartTutorial(tutorialData);
        }
    }
}
