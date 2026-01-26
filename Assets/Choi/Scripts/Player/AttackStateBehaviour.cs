using UnityEngine;

namespace Choi
{
    public class AttackStateBehaviour : StateMachineBehaviour
    {
        [Header("Hitbox Timing (Normalized Time)")]
        [Tooltip("0~1, 애니메이션 중 히트박스 활성화 시작 구간")]
        public float hitboxStart = 0.25f;

        [Tooltip("0~1, 애니메이션 중 히트박스 비활성화되는 구간")]
        public float hitboxEnd = 0.45f;

        private PlayerController player;
        private WeaponHitbox hitbox;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (player == null)
                player = animator.GetComponent<PlayerController>();

            if (hitbox == null)
                hitbox = animator.GetComponentInChildren<WeaponHitbox>();

            animator.SetBool("AttackNext", false);

            // 처음에는 히트박스 비활성화
            hitbox.DisableHitbox();
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            float t = stateInfo.normalizedTime;

            // 1) hitbox timing 처리
            if (t >= hitboxStart && t <= hitboxEnd)
                hitbox.EnableHitbox();
            else
                hitbox.DisableHitbox();

            // 2) 콤보 처리
            if (t > 0.6f)
            {
                if (player.attackQueued)
                {
                    animator.SetBool("AttackNext", true);
                    player.attackQueued = false;
                }
            }
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            hitbox.DisableHitbox();
            animator.SetBool("AttackNext", false);
        }
    }
}
