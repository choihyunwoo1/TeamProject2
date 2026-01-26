using UnityEngine;
using Choi;

namespace TeamProject2
{
    public class IdleState : State
    {
        private Animator m_Animator;

        private bool m_IsPatrol = false;
        private float m_MinTime = 0f;
        private float m_MaxTime = 3f;
        private float m_IdleTime = 0;

        readonly int m_HashForwardSpeed = Animator.StringToHash("ForwardSpeed");

        public override void OnInitalize()
        {
            // enemy는 StateMachine.SetState() 때 이미 셋팅됨
            m_Animator = enemy.GetComponent<Animator>();
        }

        public override void OnEnter()
        {
            m_Animator.SetFloat(m_HashForwardSpeed, 0f);

            if (enemy is EnemyPatrol)
            {
                m_IsPatrol = true;
                m_IdleTime = Random.Range(m_MinTime, m_MaxTime);
            }
        }

        public override void OnUpdate(float deltaTime)
        {
            if (enemy.Target)
            {

                if (enemy.IsAttackable)
                {
                    if (stateMachine.ElapseTime >= enemy.AttackDelayTime)
                    {
                        stateMachine.ChangeState(enemy.AttackState);
                    }
                }
                else
                {
                    stateMachine.ChangeState(enemy.WalkState);
                }
            }
        }
    }
}
