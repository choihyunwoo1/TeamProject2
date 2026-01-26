using UnityEngine;
using UnityEngine.UI;

namespace Choi
{
    public class StaminaBarUI : MonoBehaviour
    {
        [SerializeField] private Image fillImage;
        [SerializeField] private PlayerStats playerStats;

        private void Update()
        {
            if (playerStats == null) return;

            float percent = playerStats.CurrentStamina / playerStats.MaxStamina;
            fillImage.fillAmount = percent;
        }
    }
}
