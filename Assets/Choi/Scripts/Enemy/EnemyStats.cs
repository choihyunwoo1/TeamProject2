using UnityEngine;

namespace Choi
{
    public class EnemyStats : MonoBehaviour, IDamageable
    {
        public float health = 50f;

        public void TakeDamage(float damage)
        {
            health -= damage;
            Debug.Log("Enemy hit! Remain HP = " + health);
        }
    }
}