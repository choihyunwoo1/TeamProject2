using UnityEngine;
using Choi;

namespace TeamProject2
{
    public class BossPhase1State : State
    {
        private Animator animator;
        private bool patternFinished = false;
        // 캐싱
        private BossEnemy boss;

        //상태 초기화 함수, 상태 생성시 1회 호출
        public override void OnInitalize()
        {
            //참조
            animator = enemy.GetComponent<Animator>();
            boss = enemy as BossEnemy; // Enemy → BossEnemy 캐스팅
        }

        public override void OnEnter()
        {
            // 패턴 1 시작 연출
            patternFinished = false; // 시작 시 초기화

            animator.SetTrigger("Phase1");
        }

        public override void OnUpdate(float deltaTime)
        {
            // 패턴(애니메이션)이 끝났다고 판단되면
            if (patternFinished)
            {
                boss.IsInPhase1 = false;   // 여기서 BossEnemy 프로퍼티 접근
                stateMachine.ChangeState(enemy.IdleState);
            }
        }

        public override void OnExit()
        {
        }
    }
}
