using UnityEngine;
using UnityEngine.UI;

namespace hm
{
    public class SkillSlotUI : MonoBehaviour
    {
        [SerializeField] private Image iconImage;

        private SkillData currentSkill;
        private TooltipTrigger tooltipTrigger;

        private void Awake()
        {
            tooltipTrigger = GetComponent<TooltipTrigger>();
            Clear();
        }

        public void SetSkill(SkillData skill)
        {
            currentSkill = skill;

            iconImage.sprite = skill.icon;
            iconImage.enabled = true;

            tooltipTrigger.SetData(skill);
        }

        public void Clear()
        {
            currentSkill = null;

            iconImage.sprite = null;
            iconImage.enabled = false;

            if (tooltipTrigger != null)
                tooltipTrigger.ClearData();
        }
    }
}
