using UnityEngine;
using UnityEngine.UI;

namespace Choi
{
    public class HPBarUI : MonoBehaviour
    {
        [SerializeField] private Image fillImage;
        [SerializeField] private PlayerStats playerStats;

        private void Update()
        {
            if (playerStats == null) return;

            float percent = playerStats.CurrentHealth / playerStats.MaxHealth;
            fillImage.fillAmount = percent;
        }
    }
}
