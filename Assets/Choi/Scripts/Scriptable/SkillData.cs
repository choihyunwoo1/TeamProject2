using UnityEngine;
namespace hm
{
    [CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Objects/SkillData")]
    public class SkillData : ScriptableObject, ITooltipData
    {
        public TooltipType Type => TooltipType.Skill;

        [Header("Info")]
        public string skillName;

        [Header("Icon")]
        public Sprite icon;

        [Header("Skill Settings")]
        public float cooldown;
        public float duration;

        [Header("Tooltip")]
        public string subtitle;

        [TextArea(2, 5)]
        public string description;

        [TextArea(2, 5)]
        public string effectText;

        [TextArea(2, 5)]
        public string conditionText;
    }
}