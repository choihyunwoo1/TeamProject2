using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace hm
{
    public class WeaponUpgradeSystem : MonoBehaviour
    {
        public static WeaponUpgradeSystem Instance { get; private set; }

        [SerializeField] private Inventory inventory;
        [SerializeField] private List<WeaponUpgradeRecipe> allRecipes;

        [Header("Default Weapon")]
        [SerializeField] private WeaponItemData defaultWeapon; // 기본 무기

        private HashSet<WeaponItemData> unlockedWeapons = new();
        private Dictionary<ItemData, int> insertedMaterials = new();

        private WeaponItemData equippedWeapon;  //장착 중인 무기

        public WeaponItemData EquippedWeapon => equippedWeapon;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            // 기본 무기 자동 해금
            UnlockDefaultWeapon();

            // 인벤토리 변경 이벤트 구독
            if (inventory != null)
            {
                inventory.OnInventoryChanged += OnInventoryChanged;
            }
        }

        private void OnDestroy()
        {
            if (inventory != null)
            {
                inventory.OnInventoryChanged -= OnInventoryChanged;
            }
        }

        /// <summary>
        /// 기본 무기 자동 해금
        /// </summary>
        private void UnlockDefaultWeapon()
        {
            if (defaultWeapon != null)
            {
                unlockedWeapons.Add(defaultWeapon);

                //기본 무기 장착
                EquipWeapon(defaultWeapon);

                Debug.Log($"[기본 무기 해금] {defaultWeapon.itemName}");

                // UI 갱신
                RefreshAllUI();
            }
        }

        // 인벤토리 변경 시 UI 갱신
        private void OnInventoryChanged()
        {
            RefreshAllUI();
        }

        // 재료가 이미 삽입되었는지
        public bool IsInserted(ItemData item) => insertedMaterials.ContainsKey(item);

        // 재료 삽입
        public void InsertMaterial(ItemData item)
        {
            // 인벤토리에 해당 재료가 있는지 확인
            int available = inventory.GetItemCount(item);
            if (available <= 0)
            {
                Debug.Log($"{item.itemName}의 재고가 부족합니다.");
                return;
            }

            // 이 재료가 필요한 모든 레시피를 찾아서 최대 필요량 계산
            int maxNeeded = 0;
            foreach (var recipe in allRecipes)
            {
                // 이미 해금된 무기는 제외
                if (unlockedWeapons.Contains(recipe.targetWeapon))
                    continue;

                var mat = recipe.materials.Find(m => m.item == item);
                if (mat != null && mat.count > maxNeeded)
                    maxNeeded = mat.count;
            }

            // 어떤 레시피에도 필요하지 않은 재료
            if (maxNeeded == 0)
            {
                Debug.Log($"{item.itemName}은(는) 어떤 레시피에도 사용되지 않습니다.");
                return;
            }

            // 현재 삽입된 수량
            int currentInserted = insertedMaterials.TryGetValue(item, out int count) ? count : 0;

            // 이미 최대치까지 넣었으면 종료
            if (currentInserted >= maxNeeded)
            {
                Debug.Log($"{item.itemName}은(는) 이미 최대량이 삽입되었습니다.");
                return;
            }

            // 실제로 넣을 수량 계산
            int toInsert = Mathf.Min(maxNeeded - currentInserted, available);

            if (toInsert <= 0) return;

            // 삽입된 재료 딕셔너리 업데이트
            if (!insertedMaterials.ContainsKey(item))
                insertedMaterials[item] = 0;

            insertedMaterials[item] += toInsert;

            // 인벤토리에서 해당 수량만큼 잠금
            inventory.LockItem(item, toInsert);

            Debug.Log($"[InsertMaterial] {item.itemName} {toInsert}개 삽입 (총 {insertedMaterials[item]}개)");

            RefreshAllUI();
        }

        // 재료 제거
        public void RemoveInsertedMaterial(ItemData item)
        {
            if (!insertedMaterials.TryGetValue(item, out int insertedCount)) return;

            insertedMaterials.Remove(item);
            inventory.UnlockItem(item, insertedCount);

            Debug.Log($"[RemoveInsertedMaterial] {item.itemName} {insertedCount}개 제거 완료");

            RefreshAllUI();
        }

        // 모든 UI 갱신
        private void RefreshAllUI()
        {
            if (WeaponUpgradeUI.Instance != null)
            {
                WeaponUpgradeUI.Instance.RefreshAllRecipes(insertedMaterials, allRecipes, unlockedWeapons);
            }
        }

        // 특정 레시피가 제작 가능한지 확인
        public bool CanCraftRecipe(WeaponUpgradeRecipe recipe)
        {
            if (recipe == null) return false;
            if (unlockedWeapons.Contains(recipe.targetWeapon)) return false;

            foreach (var mat in recipe.materials)
            {
                if (!insertedMaterials.TryGetValue(mat.item, out int count) || count < mat.count)
                    return false;
            }
            return true;
        }

        // 무기 제작
        public void Craft(WeaponUpgradeRecipe recipe)
        {
            if (recipe == null)
            {
                Debug.LogError("레시피가 null입니다.");
                return;
            }

            // 제작 가능한지 최종 확인
            if (!CanCraftRecipe(recipe))
            {
                Debug.LogError("재료가 부족하거나 이미 제작된 무기입니다.");
                return;
            }

            Debug.Log($"[Craft] {recipe.targetWeapon.itemName} 제작 시작");

            // 재료 소비
            foreach (var mat in recipe.materials)
            {
                Debug.Log($"[Craft] {mat.item.itemName} {mat.count}개 소비 전 - 인벤토리: {inventory.GetItemCount(mat.item)}개, 삽입됨: {insertedMaterials[mat.item]}개");

                // 인벤토리에서 실제로 제거
                inventory.Remove(mat.item, mat.count);

                // 삽입된 재료에서도 차감
                insertedMaterials[mat.item] -= mat.count;
                if (insertedMaterials[mat.item] <= 0)
                    insertedMaterials.Remove(mat.item);

                Debug.Log($"[Craft] {mat.item.itemName} {mat.count}개 소비 후 - 인벤토리: {inventory.GetItemCount(mat.item)}개");
            }

            // 무기 해금
            unlockedWeapons.Add(recipe.targetWeapon);
            Debug.Log($"[Craft] {recipe.targetWeapon.itemName} 제작 완료!");

            // UI 갱신 (인벤토리 포함)
            RefreshAllUI();

            // 인벤토리 UI 강제 갱신 (아이템이 사라진 것을 즉시 반영)
            if (InventoryUI.Instance != null)
            {
                InventoryUI.Instance.RefreshUI();
            }
        }

        // 특정 무기의 레시피 가져오기
        public WeaponUpgradeRecipe GetRecipeByWeapon(WeaponItemData weapon)
            => allRecipes.Find(r => r.targetWeapon == weapon);

        // 무기 해금 여부
        public bool IsUnlocked(WeaponItemData weapon) => unlockedWeapons.Contains(weapon);

        // 특정 아이템의 삽입된 수량
        public int GetInsertedCount(ItemData item)
            => insertedMaterials.TryGetValue(item, out int count) ? count : 0;
        
        //장착 중인 무기
        public void EquipWeapon(WeaponItemData weapon)
        {
            equippedWeapon = weapon;
            //스킬셋 변경
            UIManager.Instance?.ChangeSkillSet(weapon.skillSet);
            RefreshAllUI(); // UI에게 상태 갱신 시킴
        }

        // 전체 재료 초기화
        public void ClearAllInsertedMaterials()
        {
            foreach (var pair in insertedMaterials.ToList())
            {
                inventory.UnlockItem(pair.Key, pair.Value);
            }
            insertedMaterials.Clear();
            RefreshAllUI();
        }
    }
}