using UnityEngine;

namespace Choi
{
    public class AttackStateBehaviour : StateMachineBehaviour
    {
        private PlayerController player;

        // 상태 진입 시 캐싱
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Debug.Log("Attack Behaviour LayerIndex = " + layerIndex);


            if (player == null)
                player = animator.GetComponent<PlayerController>();

            // 공격 진행중 표시
            animator.SetBool("AttackNext", false);
        }

        // 상태 진행 중 (매 프레임 호출)
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // 애니메이션 70% 이후에만 콤보 입력 허용
            if (stateInfo.normalizedTime > 0.6f)
            {
                // 플레이어가 공격을 큐에 넣어놨는지 확인
                if (player.attackQueued)
                {
                    animator.SetBool("AttackNext", true);
                    player.attackQueued = false;
                }
            }
        }

        // 상태 종료 시
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool("AttackNext", false);
        }
    }
}