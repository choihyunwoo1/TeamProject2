using UnityEngine;
using Choi;

namespace TeamProject2
{
    /// <summary>
    /// 적 공격 애니메이션용 히트박스 Behaviour
    /// 애니메이션 타이밍에 맞춰 EnemyHitbox 활성/비활성
    /// </summary>
    public class EnemyAttackStateBehaviour : StateMachineBehaviour
    {
        [Header("Hitbox Timing (Normalized Time)")]
        [Tooltip("0~1, 애니메이션 중 히트박스 활성화 시작 구간")]
        public float hitboxStart = 0.25f;

        [Tooltip("0~1, 애니메이션 중 히트박스 비활성화되는 구간")]
        public float hitboxEnd = 0.45f;

        private Enemy enemy;
        private EnemyHitbox hitbox;

        // 애니메이터 상태 진입 시
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (enemy == null)
                enemy = animator.GetComponent<Enemy>();

            if (hitbox == null)
                hitbox = animator.GetComponentInChildren<EnemyHitbox>();

            // 처음에는 히트박스 비활성화
            hitbox.DisableHitbox();
        }

        // 애니메이션 매 프레임 업데이트
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            float t = stateInfo.normalizedTime % 1f; // 루프 고려

            // 히트박스 활성/비활성 처리
            if (t >= hitboxStart && t <= hitboxEnd)
                hitbox.EnableHitbox();
            else
                hitbox.DisableHitbox();
        }

        // 애니메이터 상태 종료 시
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            hitbox.DisableHitbox();
        }
    }
}
