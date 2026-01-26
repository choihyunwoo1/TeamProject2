using Choi;
using System.Collections.Generic;
using UnityEngine;

namespace hm
{
    /// <summary>
    /// 무기 개조 전체 로직 담당
    /// UI / 입력 / 연출 전혀 없음
    /// </summary>
    public class WeaponUpgradeSystem : MonoBehaviour
    {
        [SerializeField] private TestInventory inventorySource; //테스트 
        private IInventory inventory;

        [Header("Current Recipe")]
        [SerializeField] private WeaponUpgradeRecipe currentRecipe;

        // 현재 레시피에 대해 채워진 재료 상태
        private Dictionary<ItemData, int> filledMaterials = new();

        // 이미 개조에 성공해서 해금된 무기들
        private HashSet<WeaponItemData> unlockedWeapons = new();

        private void Awake()
        {
            inventory = inventorySource as IInventory;
            if (inventory == null)
                Debug.LogError("Inventory does not implement IInventory");
        }

        #region Recipe Control

        public void SetRecipe(WeaponUpgradeRecipe recipe)
        {
            CancelUpgrade(); // 이전 상태 정리
            currentRecipe = recipe;
        }

        #endregion

        #region Material Fill

        /// <summary>
        /// 재료 하나를 개조 슬롯에 채운다
        /// </summary>
        public int TryFillMaterial(ItemData item)
        {
            if (currentRecipe == null)
                return 0;

            var required = GetRequiredMaterial(item);
            if (required == null)
                return 0;

            int alreadyFilled = GetFilledCount(item);
            int needCount = required.count - alreadyFilled;
            if (needCount <= 0)
                return 0;

            int ownedCount = inventory.GetItemCount(item);

            int actualCount = Mathf.Min(needCount, ownedCount);
            if (actualCount <= 0)
                return 0;

            // 인벤토리 잠금
            inventory.LockItem(item, actualCount);

            // 내부 상태 기록
            if (!filledMaterials.ContainsKey(item))
                filledMaterials[item] = 0;

            filledMaterials[item] += actualCount;

            return actualCount;
        }

        /// <summary>
        /// 재료 슬롯에서 재료를 다시 인벤으로 돌려보냄
        /// </summary>
        public int TryRemoveMaterial(ItemData item)
        {
            if (!filledMaterials.TryGetValue(item, out int filled) || filled <= 0)
                return 0;

            // 인벤 잠금 해제
            inventory.UnlockItem(item, filled);

            filledMaterials.Remove(item);

            return filled;
        }

        #endregion

        #region Condition Check

        /// <summary>
        /// 특정 재료가 레시피에 포함되는지
        /// </summary>
        private RequiredMaterial GetRequiredMaterial(ItemData item)
        {
            foreach (var mat in currentRecipe.materials)
            {
                if (mat.item == item)
                    return mat;
            }
            return null;
        }

        /// <summary>
        /// 현재 채워진 수량
        /// </summary>
        public int GetFilledCount(ItemData item)
        {
            return filledMaterials.TryGetValue(item, out int count) ? count : 0;
        }

        /// <summary>
        /// 모든 조건 충족 여부
        /// </summary>
        public bool CanUpgrade()
        {
            if (currentRecipe == null)
                return false;

            foreach (var mat in currentRecipe.materials)
            {
                if (!filledMaterials.TryGetValue(mat.item, out int count))
                    return false;

                if (count < mat.count)
                    return false;
            }

            return true;
        }

        //아이템 이미 들어갔는지 체크
        public bool IsMaterialFilled(ItemData item)
        {
            return filledMaterials.ContainsKey(item);
        }

        #endregion

        #region Upgrade Execute

        /// <summary>
        /// 무기 개조 실행
        /// </summary>
        public bool TryUpgrade()
        {
            if (!CanUpgrade())
                return false;

            // 재료 소모
            foreach (var mat in currentRecipe.materials)
            {
                inventory.RemoveItem(mat.item, mat.count);
            }

            // 무기 해금
            unlockedWeapons.Add(currentRecipe.targetWeapon);

            // 상태 초기화
            filledMaterials.Clear();

            return true;
        }

        #endregion

        #region Cancel / Reset

        /// <summary>
        /// 개조 취소 (잠금 해제)
        /// </summary>
        public void CancelUpgrade()
        {
            foreach (var pair in filledMaterials)
            {
                inventory.UnlockItem(pair.Key, pair.Value);
            }

            filledMaterials.Clear();
        }

        #endregion

        #region Unlock Info

        /// <summary>
        /// 무기 해금 여부
        /// </summary>
        public bool IsWeaponUnlocked(WeaponItemData weapon)
        {
            return unlockedWeapons.Contains(weapon);
        }

        #endregion
    }
}
