using UnityEngine;

namespace Choi
{
    [CreateAssetMenu(fileName = "PlayerStatsData", menuName = "Data/PlayerStats")]
    public class PlayerStatsDataSO : ScriptableObject
    {
        public float maxHealth = 100f;
        public float currentHealth = 100f;

        public float maxStamina = 100f;
        public float currentStamina = 100f;

        public float maxGauge = 100f;
        public float currentGauge = 0f;
    }
}