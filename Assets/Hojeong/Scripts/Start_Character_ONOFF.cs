using UnityEngine;
using UnityEngine.SceneManagement;
using Choi;

namespace HJ
{
    //마우스 올리면 Animation-Idle 재생, 클릭 시 씬 변경

    public class Start_Character_ONOFF : MonoBehaviour
    {
        //캐릭터 번호부여
        public int characterID;

        private Animator animator;

        public void Start()
        {
            animator = GetComponent<Animator>();
        }
        private void OnMouseEnter()
        {
            animator.SetBool("IsActive", true);
            SoundManager.Instance.Play("StartButton");
        }

        private void OnMouseExit()
        {
            animator.SetBool("IsActive", false);
        }

        private void OnMouseDown()
        {   
            
            //남캐 : 0 // 여캐 :1 // 로 설정해두었습니다.
            //번호 선택시 DataManager에 정보 저장
            DataManager.selectedCharacter = characterID;
            
            //ScemeManager 이용해서 씬 변경    //작성해주세요
            SceneManager.LoadScene("Test_NextScene");
        }

    }
}