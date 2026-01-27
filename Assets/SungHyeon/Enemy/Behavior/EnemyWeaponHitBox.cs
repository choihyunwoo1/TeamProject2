using Choi;
using System.Collections.Generic;
using UnityEngine;

namespace TeamProject2
{
    /// <summary>
    /// 적 공격용 히트박스
    /// 공격 시점에 EnableHitbox(), 끝나면 DisableHitbox() 호출
    /// 한 공격에서 한 플레이어에만 데미지 적용
    /// </summary>
    public class EnemyHitbox : MonoBehaviour
    {
        private Collider hitbox;
        private Enemy enemy;
        private HashSet<IDamageable> hitTargets = new HashSet<IDamageable>();

        private void Awake()
        {
            hitbox = GetComponent<Collider>();
            enemy = GetComponentInParent<Enemy>();
            hitbox.enabled = false;
        }

        public void EnableHitbox()
        {
            hitbox.enabled = true;
            hitTargets.Clear(); // 공격 시작 시 초기화
        }

        public void DisableHitbox()
        {
            hitbox.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            // 플레이어만 공격
            PlayerStats player = other.GetComponentInParent<PlayerStats>();
            if (player == null)
                return;

            // 이미 한 번 맞은 대상이면 무시
            if (hitTargets.Contains(player))
                return;

            hitTargets.Add(player);

            // 데미지 적용
            player.TakeDamage(10f); // 적 공격력 예시, 필요하면 Enemy에서 가져오도록 변경
        }
    }
}
