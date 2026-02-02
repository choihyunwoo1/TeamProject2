using System.Collections.Generic;
using UnityEngine;

namespace hm
{
    [CreateAssetMenu(menuName = "Skill/SkillSet")]
    public class SkillSetData : ScriptableObject
    {
        public WeaponType weaponType;
        public List<SkillData> skills;
    }
}
