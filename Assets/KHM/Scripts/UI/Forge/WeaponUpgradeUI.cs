using System.Collections.Generic;
using UnityEngine;

namespace hm
{
    public class WeaponUpgradeUI : MonoBehaviour
    {
        public static WeaponUpgradeUI Instance { get; private set; }

        // 3개의 레시피 슬롯 그룹 (각 레시피당 3개의 재료 슬롯)
        [Header("Recipe 1")]
        [SerializeField] private List<UpgradeMaterialSlotUI> recipe1Slots;
        [SerializeField] private WeaponButtonUI weapon1Button;

        [Header("Recipe 2")]
        [SerializeField] private List<UpgradeMaterialSlotUI> recipe2Slots;
        [SerializeField] private WeaponButtonUI weapon2Button;

        [Header("Recipe 3")]
        [SerializeField] private List<UpgradeMaterialSlotUI> recipe3Slots;
        [SerializeField] private WeaponButtonUI weapon3Button;

        //기본무기
        [SerializeField] private WeaponButtonUI defaultWeaponButton;

        [Header("Inserted Materials Display")]
        [SerializeField] private List<InsertedMaterialSlotUI> insertedSlots;

        [Header("UI Roots")]
        [SerializeField] private GameObject upgradeUIRoot;
        [SerializeField] private GameObject inventoryUIRoot;
        [SerializeField] private GameObject inventoryPopupUI;
        [SerializeField] private WeaponPopupUI weaponPopupPrefab; // 프리팹
        private WeaponPopupUI weaponPopupInstance;                  // 인스턴스

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            // 초기화 - 모든 레시피 표시
            InitializeAllRecipes();
        }

        // 모든 레시피 초기화 및 표시
        private void InitializeAllRecipes()
        {
            var system = WeaponUpgradeSystem.Instance;
            if (system == null) return;

            // 무기 버튼에서 레시피 가져오기
            if (weapon1Button != null && weapon1Button.Weapon != null)
            {
                var recipe1 = system.GetRecipeByWeapon(weapon1Button.Weapon);
                if (recipe1 != null) InitRecipeSlots(recipe1Slots, recipe1);
            }

            if (weapon2Button != null && weapon2Button.Weapon != null)
            {
                var recipe2 = system.GetRecipeByWeapon(weapon2Button.Weapon);
                if (recipe2 != null) InitRecipeSlots(recipe2Slots, recipe2);
            }

            if (weapon3Button != null && weapon3Button.Weapon != null)
            {
                var recipe3 = system.GetRecipeByWeapon(weapon3Button.Weapon);
                if (recipe3 != null) InitRecipeSlots(recipe3Slots, recipe3);
            }
        }

        // 레시피 슬롯 초기화
        private void InitRecipeSlots(List<UpgradeMaterialSlotUI> slots, WeaponUpgradeRecipe recipe)
        {
            if (slots == null || recipe == null) return;

            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i] == null) continue;

                if (i < recipe.materials.Count)
                    slots[i].Init(recipe.materials[i]);
                else
                    slots[i].Clear();
            }
        }

        // 모든 레시피 갱신 (재료를 넣거나 뺄 때 호출)
        public void RefreshAllRecipes(
            Dictionary<ItemData, int> insertedMaterials,
            List<WeaponUpgradeRecipe> allRecipes,
            HashSet<WeaponItemData> unlockedWeapons)
        {
            if (insertedMaterials == null) return;

            // 각 레시피 그룹 갱신
            if (weapon1Button != null && weapon1Button.Weapon != null)
            {
                var recipe1 = allRecipes.Find(r => r.targetWeapon == weapon1Button.Weapon);
                RefreshRecipeGroup(recipe1Slots, weapon1Button, recipe1, insertedMaterials, unlockedWeapons);
            }

            if (weapon2Button != null && weapon2Button.Weapon != null)
            {
                var recipe2 = allRecipes.Find(r => r.targetWeapon == weapon2Button.Weapon);
                RefreshRecipeGroup(recipe2Slots, weapon2Button, recipe2, insertedMaterials, unlockedWeapons);
            }

            if (weapon3Button != null && weapon3Button.Weapon != null)
            {
                var recipe3 = allRecipes.Find(r => r.targetWeapon == weapon3Button.Weapon);
                RefreshRecipeGroup(recipe3Slots, weapon3Button, recipe3, insertedMaterials, unlockedWeapons);
            }

            // 삽입된 재료 표시 갱신
            RefreshInsertedSlots(insertedMaterials);

            // 인벤토리 UI 갱신 (삽입된 재료 마스크 표시)
            if (InventoryUI.Instance != null)
            {
                InventoryUI.Instance.RefreshUI();
            }

            //무기 장착 중 표시
            RefreshEquippedStates();
        }

        //무기 장착 중 표시
        private void RefreshEquippedStates()
        {
            var equipped = WeaponUpgradeSystem.Instance.EquippedWeapon;

            weapon1Button?.RefreshEquipped(equipped);
            weapon2Button?.RefreshEquipped(equipped);
            weapon3Button?.RefreshEquipped(equipped);
            defaultWeaponButton?.RefreshEquipped(equipped);
        }

        // 개별 레시피 그룹 갱신
        private void RefreshRecipeGroup(
            List<UpgradeMaterialSlotUI> recipeSlots,
            WeaponButtonUI weaponButton,
            WeaponUpgradeRecipe recipe,
            Dictionary<ItemData, int> insertedMaterials,
            HashSet<WeaponItemData> unlockedWeapons)
        {
            if (recipe == null || recipeSlots == null || weaponButton == null) return;

            bool allMaterialsSatisfied = true;

            // 각 재료 슬롯 갱신
            foreach (var slot in recipeSlots)
            {
                if (slot == null) continue;

                ItemData item = slot.GetItem();
                if (item == null) continue;

                // 현재 삽입된 수량 가져오기
                int insertedCount = insertedMaterials.TryGetValue(item, out int count) ? count : 0;

                // 슬롯 갱신 (마스크 자동 처리)
                slot.Refresh(insertedCount);

                // 필요한 수량 가져오기
                var requiredMat = recipe.materials.Find(m => m.item == item);
                if (requiredMat != null && insertedCount < requiredMat.count)
                {
                    allMaterialsSatisfied = false;
                }
            }

            // 무기 버튼 상태 갱신
            bool isUnlocked = unlockedWeapons.Contains(recipe.targetWeapon);
            weaponButton.SetCraftable(isUnlocked || allMaterialsSatisfied);
        }

        // 삽입된 재료 표시 갱신
        private void RefreshInsertedSlots(Dictionary<ItemData, int> insertedMaterials)
        {
            if (insertedSlots == null || insertedMaterials == null) return;

            int index = 0;
            foreach (var pair in insertedMaterials)
            {
                if (index >= insertedSlots.Count) break;
                if (insertedSlots[index] != null)
                    insertedSlots[index].Set(pair.Key, pair.Value);
                index++;
            }

            // 나머지 슬롯은 비우기
            for (; index < insertedSlots.Count; index++)
            {
                if (insertedSlots[index] != null)
                    insertedSlots[index].Clear();
            }
        }

        // 무기 개조창 열기
        public void OpenUpgradeUI()
        {
            if (upgradeUIRoot != null) upgradeUIRoot.SetActive(true);
            if (inventoryUIRoot != null) inventoryUIRoot.SetActive(true);

            // 인벤토리를 무기개조 모드로 설정
            if (InventoryUI.Instance != null)
            {
                InventoryUI.Instance.SetMode(InventoryMode.WeaponUpgrade);
            }
        }

        //무기 선택 팝업창 열기
        public void OpenWeaponPopup(WeaponItemData weapon, RectTransform buttonRect)
        {
            // 인벤토리 팝업이 열려있으면 닫기
            if (inventoryPopupUI != null && inventoryPopupUI.activeSelf)
            {
                inventoryPopupUI.SetActive(false);
            }

            // 인벤토리 선택 상태 초기화
            if (InventoryUI.Instance != null)
            {
                InventoryUI.Instance.ClearSelection();
            }

            if (weaponPopupInstance == null)
            {
                // UIHierarchy상 WeaponUpgradeUI 아래에 생성
                weaponPopupInstance = Instantiate(weaponPopupPrefab, transform);
            }

            weaponPopupInstance.gameObject.SetActive(true);
            weaponPopupInstance.Open(weapon, buttonRect);
        }

        // ⭐️ 무기 팝업 닫기 (인벤토리 슬롯 클릭 시 호출)
        public void CloseWeaponPopup()
        {
            if (weaponPopupInstance != null && weaponPopupInstance.gameObject.activeSelf)
            {
                weaponPopupInstance.gameObject.SetActive(false);
            }
        }

        // 모든 UI 닫기
        public void CloseAllUI()
        {
            if (upgradeUIRoot != null) upgradeUIRoot.SetActive(false);
            if (inventoryUIRoot != null) inventoryUIRoot.SetActive(false);

            if (weaponPopupInstance != null)
                weaponPopupInstance.gameObject.SetActive(false);

            if (inventoryPopupUI != null && inventoryPopupUI.activeSelf)
                inventoryPopupUI.SetActive(false);

            if (InventoryUI.Instance != null)
            {
                InventoryUI.Instance.SetMode(InventoryMode.Normal);
            }
        }

        // 무기 개조 팝업이 열려있는지 확인
        public bool IsWeaponPopupOpen()
        {
            return weaponPopupInstance != null && weaponPopupInstance.gameObject.activeSelf;
        }
    }
}