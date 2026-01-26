using UnityEngine;
using UnityEngine.UI;

namespace hm
{
    public class WeaponUpgradeUI : MonoBehaviour
    {
        [SerializeField] private WeaponUpgradeSystem upgradeSystem;
        [SerializeField] private UpgradeMaterialSlotUI[] materialSlots;
        [SerializeField] private Button upgradeButton;

        private WeaponUpgradeRecipe currentRecipe;

        public void SetRecipe(WeaponUpgradeRecipe recipe)
        {
            currentRecipe = recipe;
            upgradeSystem.SetRecipe(recipe);

            Refresh();
        }

        public void Refresh()
        {
            if (currentRecipe == null)
                return;

            //재료 슬롯 갱신
            for (int i = 0; i < materialSlots.Length; i++)
            {
                if (i >= currentRecipe.materials.Count)
                {
                    materialSlots[i].Clear();
                    continue;
                }

                var mat = currentRecipe.materials[i];
                int filled = upgradeSystem.GetFilledCount(mat.item);

                materialSlots[i].Set(mat.item, filled, mat.count);
            }

            //버튼 활성화
            upgradeButton.interactable = upgradeSystem.CanUpgrade();
        }

        public void OnClickUpgrade()
        {
            if (upgradeSystem.TryUpgrade())
            {
                Refresh();
                // 성공 연출
            }
        }
    }

}