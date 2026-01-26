using UnityEngine;
using Choi;

namespace TeamProject2
{
    public class GetHitState : State
    {
        #region Variables
        //참조
        private Animator m_Ainmator;

        #endregion

        public override void OnInitalize()
        { 
            //참조
            m_Ainmator = enemy.GetComponent<Animator>();
        }

        public override void OnEnter()
        { 
        }

        public override void OnUpdate(float deltaTime)
        {
            m_Ainmator.SetTrigger("GetHit");
        }
    }
}