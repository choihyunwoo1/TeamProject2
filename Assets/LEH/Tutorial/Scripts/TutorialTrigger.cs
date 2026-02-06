using Choi;
using System.Collections.Generic;
using UnityEngine;

namespace hm
{
    public class TutorialTrigger : MonoBehaviour
    {
        [Header("Tutorial Data")]
        public TutorialData tutorialData;

        TutorialManager tutorialManager;
        bool triggered = false;

        [Header("UI Activation")]
        public GameObject tutorialUIToOpen;          // 하나만 띄울 때
        public List<GameObject> extraUIToActivate;   // 여러 개 띄울 때 옵션

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

            triggered = true;

            if (tutorialUIToOpen != null)
                tutorialUIToOpen.SetActive(true);

            // 필요하면 여러 개도 가능
            foreach (var ui in extraUIToActivate)
                if (ui != null) ui.SetActive(true);

            // 튜토리얼 시작
            tutorialManager.StartTutorial(tutorialData);
        }
    }
}