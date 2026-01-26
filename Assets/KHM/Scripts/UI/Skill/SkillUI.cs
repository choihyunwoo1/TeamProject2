using UnityEngine;
using UnityEngine.InputSystem;

namespace Choi
{
    public class SkillUI : MonoBehaviour
    {
        [SerializeField] private SkillSlotUI[] slots;

        /// <summary>
        /// 무기 변경 시 UIManager가 호출하는 유일한 함수
        /// </summary>
        
        //테스트용 치트키 
        public SkillSetData testSkillSet;

        void Update()
        {
            if (Keyboard.current.kKey.wasPressedThisFrame)
            {
                UIManager.Instance.ChangeSkillSet(testSkillSet);
            }
        }

        public void SetSkillSet(SkillSetData skillSet)
        {
            if (skillSet == null)
            {
                ClearAll();
                return;
            }

            for (int i = 0; i < slots.Length; i++)
            {
                if (i < skillSet.skills.Count && skillSet.skills[i] != null)
                {
                    slots[i].SetSkill(skillSet.skills[i]);
                }
                else
                {
                    slots[i].Clear();
                }
            }
        }

        private void ClearAll()
        {
            foreach (var slot in slots)
                slot.Clear();
        }
    }
}

