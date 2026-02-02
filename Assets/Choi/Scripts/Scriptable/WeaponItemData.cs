using UnityEngine;

namespace hm
{
    [CreateAssetMenu(fileName = "WeaponItemData", menuName = "Item/Weapon")]
    public class WeaponItemData : ItemData
    {
        public SkillSetData skillSet;
    }
}