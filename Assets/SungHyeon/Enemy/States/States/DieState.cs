using UnityEngine;
using Choi;

namespace TeamProject2
{
    /// <summary>
    /// 죽는 상태를 관리하는 클래스
    /// </summary>
    public class DieState : State
    {
        #region Variables
        //참조
        private Animator m_Animator;
        #endregion

        //상태 초기화 함수, 상태 생성시 1회 호출
        public override void OnInitalize()
        {
            //참조
            m_Animator = enemy.GetComponent<Animator>();
        }

        //상태 들어가기, 상태 들어갈때 마다 1회 호출
        public override void OnEnter()
        {
            //애니메이터 설정
            m_Animator.SetTrigger("Die");
        }

        //상태 업데이트, 매 프레임 마다 호출
        public override void OnUpdate(float deltaTime)
        {

        }
    }
}