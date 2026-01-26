using UnityEngine;

namespace TeamProject2
{
    public class GetHitState : State
    {
        #region Variables
        //참조
        private Animator m_Ainmator;

        //애니메이터 파라미터
        readonly int m_HashGetHit = Animator.StringToHash("GetHit");
        #endregion

        public override void OnInitalize()
        { 
            //참조
            m_Ainmator = enemy.GetComponent<Animator>();
        }

        //
        public override void OnEnter()
        { 
            //
            m_Ainmator.SetTrigger(m_HashGetHit);
        }

        public override void OnUpdate(float deltaTime)
        {
            
        }
    }
}