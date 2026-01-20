using System.Collections.Generic;
using UnityEngine;

namespace Choi
{
    public class WeaponHitbox : MonoBehaviour
    {
        private Collider hitbox;
        private PlayerStats ownerStats;

        private HashSet<IDamageable> hitEnemies = new HashSet<IDamageable>();

        void Awake()
        {
            hitbox = GetComponent<Collider>();
            ownerStats = GetComponentInParent<PlayerStats>();

            hitbox.enabled = false;
        }

        public void EnableHitbox()
        {
            hitbox.enabled = true;
            hitEnemies.Clear();
        }

        public void DisableHitbox()
        {
            hitbox.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            // 자기 자신 무시
            if (other.GetComponentInParent<PlayerStats>() == ownerStats)
                return;

            // 대상 찾기
            IDamageable dmg = other.GetComponentInParent<IDamageable>();
            if (dmg == null)
                return;

            if (hitEnemies.Contains(dmg))
                return;
            hitEnemies.Add(dmg);

            dmg.TakeDamage(10f);
            ownerStats.AddGauge(10f);
        }
    }
}
