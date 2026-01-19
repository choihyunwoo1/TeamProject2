using System.Collections.Generic;
using UnityEngine;

namespace Choi
{
    public class WeaponHitbox : MonoBehaviour
    {
        private Collider hitbox;
        private PlayerStats ownerStats;

        private HashSet<IDamageable> hitEnemies = new HashSet<IDamageable>();
        private float activationTime;

        void Awake()
        {
            hitbox = GetComponent<Collider>();
            hitbox.enabled = false;

            ownerStats = GetComponentInParent<PlayerStats>();
            Debug.Log("ownerStats is NULL? = " + (ownerStats == null));
        }

        public void EnableHitbox()
        {
            activationTime = Time.time;
            hitbox.enabled = true;
            hitEnemies.Clear();
        }

        public void DisableHitbox()
        {
            hitbox.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("root = " + other.transform.root.name);
            Debug.Log("parents = " + other.transform.parent);
            Debug.Log("has PlayerStats in parents? = " + (other.GetComponentInParent<PlayerStats>() != null));


            // 활성화 즉시 튀는 입력 무시
            if (Time.time - activationTime < 0.02f)
                return;

            // 자기 자신 무시
            if (other.GetComponentInParent<PlayerStats>() == ownerStats)
                return;
            Debug.Log("1");

            // 대상 찾기
            IDamageable dmg = other.GetComponentInParent<IDamageable>();
            if (dmg == null)
                return;

            if (hitEnemies.Contains(dmg))
                return;
            Debug.Log("2");
            hitEnemies.Add(dmg);

            dmg.TakeDamage(10f);
            ownerStats.AddGauge(10f);
            Debug.Log("3");
        }
    }
}
