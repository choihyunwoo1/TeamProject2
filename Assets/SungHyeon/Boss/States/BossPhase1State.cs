using UnityEngine;
using TeamProject2;

namespace Choi
{
    public class BossPhase1State : State
    {
        private bool patternFinished = false;  // ← 반드시 선언 필요

        public override void OnEnter()
        {
            // 패턴 1 시작 연출
            patternFinished = false; // 시작 시 초기화
        }

        public override void OnUpdate(float deltaTime)
        {
            // 패턴(애니메이션)이 끝났다고 판단되면
            if (patternFinished)
            {
                enemy.IsInPhase1 = false; // Phase1 해제
                stateMachine.ChangeState(enemy.IdleState);
            }
        }

        public override void OnExit()
        {
            // 정리
        }
    }
}
