using UnityEngine;
using Choi;

namespace TeamProject2
{
    public class BossDamageZone : MonoBehaviour
    {

        public float damage = 20f;
        public float duration = 2f;  // 구역 유지 시간

        private void Start()
        {
            Destroy(gameObject, duration);
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("player init");

            // 플레이어만 공격
            PlayerStats player = other.GetComponentInParent<PlayerStats>();
            if (player == null)
                return;

            // 데미지 적용
            player.TakeDamage(damage); // 적 공격력 예시, 필요하면 Enemy에서 가져오도록 변경
        }
    }
}