using UnityEngine;
using System.Collections;

namespace Choi
{
    public class Portal : MonoBehaviour
    {
        [Header("Portal Settings")]
        [SerializeField] private string sceneToLoad;
        [SerializeField] private float requiredStayTime = 5f;

        [Header("Portal ID")]
        [SerializeField] private string portalID;          // 현재 포탈의 ID
        [SerializeField] private string targetPortalID;    // 다음 씬에서 도착할 포탈 ID

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

            PortalManager.LastPortalID = targetPortalID;

            //씬 이동 직전에 기존 플레이어 삭제
            var player = FindFirstObjectByType<PlayerStats>();
            if (player != null)
                Destroy(player.gameObject);

            //페이드 후 씬 이동
            if (sceneFader != null && !string.IsNullOrEmpty(sceneToLoad))
            {
                sceneFader.FadeTo(sceneToLoad);
            }
        }
    }
}
