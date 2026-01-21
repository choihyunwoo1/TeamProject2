using UnityEngine;
using System.Collections;

namespace Choi
{
    public class Portal : MonoBehaviour
    {
        [Header("Portal Settings")]
        [SerializeField] private string sceneToLoad;
        [SerializeField] private float requiredStayTime = 5f;

        [Header("References")]
        [SerializeField] private SceneFader sceneFader;

        private Coroutine timerCoroutine;
        private bool isPlayerInside = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerInside = true;
                timerCoroutine = StartCoroutine(PortalTimer());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerInside = false;

                if (timerCoroutine != null)
                {
                    StopCoroutine(timerCoroutine);
                    timerCoroutine = null;
                }
            }
        }

        private IEnumerator PortalTimer()
        {
            float elapsed = 0f;

            while (elapsed < requiredStayTime)
            {
                if (!isPlayerInside)
                    yield break;

                elapsed += Time.deltaTime;
                yield return null;
            }

            // 5초 이상 포탈 내부 → SceneFader 사용해 페이드 아웃 후 씬 이동
            if (sceneFader != null && !string.IsNullOrEmpty(sceneToLoad))
            {
                sceneFader.FadeTo(sceneToLoad);
            }
        }
    }
}
