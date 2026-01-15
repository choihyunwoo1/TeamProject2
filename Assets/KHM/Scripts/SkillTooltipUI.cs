using TMPro;
using UnityEngine;
namespace hm
{
    public class SkillTooltipUI : TooltipUIBase
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descriptionText;

        public override void Show(ITooltipData data)
        {
            var skillData = (SkillData)data;

            nameText.text = skillData.skillName;
            descriptionText.text = skillData.description;

            gameObject.SetActive(true);
        }
    }
}