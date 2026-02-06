using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Choi
{
    public class StaminaBarUI : MonoBehaviour
    {
        [SerializeField] private Image fillImage;
        private PlayerStats playerStats;

        private void Start()
        {
            StartCoroutine(WaitForPlayer());
        }

        private IEnumerator WaitForPlayer()
        {
            while (playerStats == null)
            {
                playerStats = FindFirstObjectByType<PlayerStats>();
                yield return null;
            }
        }

        private void Update()
        {
            if (playerStats == null) return;

            float percent = playerStats.CurrentStamina / playerStats.MaxStamina;
            fillImage.fillAmount = percent;
        }
    }
}
