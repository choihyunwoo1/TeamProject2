using UnityEngine;
using UnityEngine.UI;

namespace Choi
{
    public class SkillGaugeBar : MonoBehaviour
    {
        [SerializeField] private Image fillImage;
        [SerializeField] private PlayerStats playerStats;

        private void Update()
        {
            if (playerStats == null) return;

            float percent = playerStats.currentGauge / playerStats.maxGauge;
            fillImage.fillAmount = percent;
        }
    }
}