using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Choi
{
    public class HPBarUI : MonoBehaviour
    {
        [SerializeField] private Image fillImage;
        private PlayerStats playerStats;

        private void Start()
        {
            StartCoroutine(WaitForPlayer());
        }

        private IEnumerator WaitForPlayer()
        {
            // 플레이어가 Instantiate될 때까지 반복적으로 검색
            while (playerStats == null)
            {
                playerStats = FindFirstObjectByType<PlayerStats>();
                yield return null; // 다음 프레임까지 대기
            }
        }

        private void Update()
        {
            if (playerStats == null) return;

            float percent = playerStats.CurrentHealth / playerStats.MaxHealth;
            fillImage.fillAmount = percent;
        }
    }
}
