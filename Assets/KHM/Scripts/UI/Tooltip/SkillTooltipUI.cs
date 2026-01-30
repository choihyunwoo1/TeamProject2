using TMPro;
using UnityEngine;
namespace hm
{
    public class SkillTooltipUI : TooltipUIBase
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private TMP_Text flavorText;
        [SerializeField] private TMP_Text subtitleText;

        public override void Show(ITooltipData data)
        {
            var skillData = (SkillData)data;
            string _flavorText = $"{skillData.effectText}, {skillData.conditionText}";


            nameText.text = skillData.skillName;
            descriptionText.text = skillData.description;
            flavorText.text = _flavorText;
            subtitleText.text = skillData.subtitle;

            gameObject.SetActive(true);
        }
    }
}