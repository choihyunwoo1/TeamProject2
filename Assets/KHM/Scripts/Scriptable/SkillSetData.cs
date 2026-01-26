using System.Collections.Generic;
using UnityEngine;

namespace Choi
{
    [CreateAssetMenu(menuName = "Skill/SkillSet")]
    public class SkillSetData : ScriptableObject
    {
        public WeaponType weaponType;
        public List<SkillData> skills;
    }
}
