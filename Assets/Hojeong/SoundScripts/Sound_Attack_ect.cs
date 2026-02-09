using UnityEngine;

namespace HJ
{
    public class PlaySoundOnEnter : StateMachineBehaviour
    {
        public string soundName;
        public GameObject showScreen;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //사운드 전부 플레이
            if (SoundManager.Instance == null) return;
            SoundManager.Instance.Play(soundName);

            // 강공격
            if (soundName == "L_Attack")
            {
                SoundManager.Instance.SetPitch("L_Attack", 1.7f);
            }
        }

        //공격 멈추면
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (soundName == "L_Attack" && !animator.GetBool("IsAttacking"))
            {
                SoundManager.Instance.Stop("L_Attack");
                SoundManager.Instance.SetPitch("L_Attack", 1f); // 피치 원상복구
            }
        }
        
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (soundName == "L_Attack")
            {
                SoundManager.Instance.SetPitch("L_Attack", 1f);
                SoundManager.Instance.Stop("L_Attack");
            }
        }
    }
}