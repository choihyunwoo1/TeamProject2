using System;
using System.Collections.Generic;
using UnityEngine;
namespace hm
{
    [Serializable]
    public class RequiredMaterial
    {
        public ItemData item;
        public int count;
    }

    [CreateAssetMenu(fileName = "WeaponUpgradeRecipe", menuName = "Game/Weapon Upgrade Recipe")]
    public class WeaponUpgradeRecipe : ScriptableObject
    {
        public WeaponItemData targetWeapon;
        public List<RequiredMaterial> materials = new();
       
    }
}