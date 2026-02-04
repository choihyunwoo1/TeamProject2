using System.Collections.Generic;
using UnityEngine;

namespace Choi
{
    public enum DamageType
    {
        Normal,
        Strong,
        Ultimate
    }

    public class WeaponHitbox : MonoBehaviour
    {
        private Collider hitbox;
        private PlayerStats ownerStats;

        public DamageType damageType = DamageType.Normal;   // 공격 타입
        public float baseDamage = 10f;

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

            //공격 타입과 값을 Damageable에게 넘긴다
            dmg.TakeDamage(baseDamage, damageType);

            ownerStats.AddGauge(10f);
        }
    }
}
