using UnityEngine;
namespace hm
{
    [CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Objects/SkillData")]
    public class SkillData : ScriptableObject, ITooltipData
    {
        public TooltipType Type => TooltipType.Skill;

        [Header("Info")]
        public string skillName;

        [TextArea(2, 4)]
        public string description;

        [Header("Icon")]
        public Sprite icon;

        [Header("Skill Settings")]
        public float cooldown;
        public float duration;
    }
}