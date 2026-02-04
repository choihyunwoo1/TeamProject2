using UnityEngine;

namespace hm
{
    /// <summary>
    /// 인벤토리의 현재 상태를 정의하는 enum
    /// </summary>
    public enum InventoryMode
    {
        Normal,         // 기본 상태 - 모든 아이템 사용 가능
        Shop,           // 상점 상태 - 아이템 판매/구매
        WeaponUpgrade   // 무기개조 상태 - Material만 사용 가능
    }

    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private InventoryItemUI[] slots;
        [SerializeField] private UpgradeSelectPopupUI popupUI;
        [SerializeField] private WeaponUpgradeSystem upgradeSystem;
        [SerializeField] private WeaponUpgradeUI upgradeUI;
        [SerializeField] private GameObject exitButton;

        public static InventoryUI Instance { get; private set; }

        // 인벤토리 현재 모드
        private InventoryMode currentMode = InventoryMode.Normal;
        // 선택 중인 슬롯
        private InventoryItemUI currentSelectedSlot;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            Inventory.Instance.OnInventoryChanged += RefreshUI;
            RefreshUI();
        }

        private void OnDestroy()
        {
            if (Inventory.Instance != null)
                Inventory.Instance.OnInventoryChanged -= RefreshUI;
        }

        #region Mode Management

        /// <summary>
        /// 인벤토리 모드 설정
        /// </summary>
        public void SetMode(InventoryMode mode)
        {
            currentMode = mode;
            Debug.Log($"인벤토리 모드 변경: {mode}");

            //무기개조, 상점 모드 일 때 exit버튼 비활성화
            if (exitButton != null)
            {
                exitButton.SetActive(mode == InventoryMode.Normal);
            }

            // 모드 변경 시 선택 상태 초기화
            ClearSelection();

            // ⭐️ 모드 변경 시 퀵슬롯 선택 모드 종료
            if (mode != InventoryMode.Normal)
            {
                UIManager.Instance?.ExitQuickSlotSelectMode();
            }

            // 모드 변경 시 UI 갱신
            RefreshUI();
            RefreshAllButtonStates();
        }

        /// <summary>
        /// 현재 인벤토리 모드 반환
        /// </summary>
        public InventoryMode GetCurrentMode() => currentMode;

        /// <summary>
        /// 무기개조 모드인지 확인
        /// </summary>
        public bool IsUpgradeMode() => currentMode == InventoryMode.WeaponUpgrade;

        /// <summary>
        /// 상점 모드인지 확인
        /// </summary>
        public bool IsShopMode() => currentMode == InventoryMode.Shop;

        /// <summary>
        /// 일반 모드인지 확인
        /// </summary>
        public bool IsNormalMode() => currentMode == InventoryMode.Normal;

        #endregion

        #region UI Refresh

        /// <summary>
        /// UI 갱신 - 슬롯 바인딩 및 시각 업데이트
        /// </summary>
        public void RefreshUI()
        {
            var allSlots = Inventory.Instance.GetSlots();

            for (int i = 0; i < slots.Length; i++)
            {
                if (i < allSlots.Count)
                {
                    // Bind를 호출하면 내부에서 UpdateVisual이 자동 호출됨
                    slots[i].Bind(allSlots[i]);
                }
                else
                {
                    slots[i].Clear();
                }
            }

            // 모드별 추가 처리
            switch (currentMode)
            {
                case InventoryMode.WeaponUpgrade:
                    UpdateMaterialMasks();
                    break;

                case InventoryMode.Shop:
                    // 상점 모드는 추가 처리 없음
                    break;

                case InventoryMode.Normal:
                default:
                    // 일반 모드는 추가 처리 없음
                    break;
            }
        }

        /// <summary>
        /// 삽입된 재료의 마스크 업데이트 (무기개조 모드)
        /// </summary>
        private void UpdateMaterialMasks()
        {
            if (upgradeSystem == null) return;

            foreach (var slotUI in slots)
            {
                if (slotUI == null || slotUI.ItemData == null) continue;

                // 재료 아이템만 체크
                if (slotUI.ItemData.itemType == ItemType.Material)
                {
                    // 삽입된 수량 확인
                    int insertedCount = upgradeSystem.GetInsertedCount(slotUI.ItemData);

                    // 삽입된 재료가 있으면 마스크 표시
                    if (insertedCount > 0)
                    {
                        slotUI.SetMask(true);
                    }
                }
            }
        }

        /// <summary>
        /// 아이템 슬롯 선택 효과
        /// </summary>
        public void SetSelectedSlot(InventoryItemUI slot)
        {
            // 이전 선택 해제
            if (currentSelectedSlot != null)
                currentSelectedSlot.SetSelected(false);

            currentSelectedSlot = slot;

            if (currentSelectedSlot != null)
                currentSelectedSlot.SetSelected(true);
        }

        public void ClearSelection()
        {
            if (currentSelectedSlot != null)
            {
                currentSelectedSlot.SetSelected(false);
                currentSelectedSlot = null;
            }
        }

        /// <summary>
        /// 모든 슬롯의 버튼 상태 갱신
        /// </summary>
        private void RefreshAllButtonStates()
        {
            foreach (var slotUI in slots)
            {
                if (slotUI != null)
                {
                    slotUI.RefreshButtonState();
                }
            }
        }

        #endregion

        #region Item Click Handling

        /// <summary>
        /// 아이템 클릭 시 처리 (모드별로 다른 동작)
        /// </summary>
        /// <param name="item">클릭한 아이템</param>
        /// <param name="slotTransform">클릭한 슬롯의 RectTransform (팝업 위치 설정용)</param>
        public void OnItemClicked(ItemData item, RectTransform slotTransform = null)
        {
            if (item == null) return;

            switch (currentMode)
            {
                case InventoryMode.Normal:
                    HandleNormalModeClick(item);
                    break;

                case InventoryMode.WeaponUpgrade:
                    HandleWeaponUpgradeModeClick(item, slotTransform);
                    break;

                case InventoryMode.Shop:
                    HandleShopModeClick(item, slotTransform);
                    break;
            }
        }

        /// <summary>
        /// 일반 모드에서 아이템 클릭 처리
        /// </summary>
        private void HandleNormalModeClick(ItemData item)
        {
            // 일반 모드일 때만 퀵슬롯 등록 가능
            if (currentMode != InventoryMode.Normal)
            {
                Debug.Log("일반 모드에서만 퀵슬롯 등록이 가능합니다.");
                return;
            }

            // 소비 아이템이면 퀵슬롯에 등록
            if (item.category == ItemCategory.UseItem)
            {
                UIManager.Instance?.SelectItemForQuickSlot(item);
            }
            // 다른 아이템 타입 처리 추가 가능
            else
            {
                Debug.Log($"{item.itemName} 클릭됨 (일반 모드)");
            }
        }

        /// <summary>
        /// 무기개조 모드에서 아이템 클릭 처리
        /// </summary>
        private void HandleWeaponUpgradeModeClick(ItemData item, RectTransform slotTransform)
        {
            // Material 아이템만 팝업 열기
            if (item.itemType == ItemType.Material)
            {
                // ⭐️ 무기 팝업이 열려있으면 닫기
                if (upgradeUI != null && upgradeUI.IsWeaponPopupOpen())
                {
                    upgradeUI.CloseWeaponPopup();
                }

                // RectTransform을 함께 전달하여 팝업 위치 설정
                popupUI.Open(item, upgradeSystem, upgradeUI, slotTransform);
            }
            else
            {
                Debug.Log($"{item.itemName}은(는) 무기개조에 사용할 수 없습니다.");
            }
        }

        /// <summary>
        /// 상점 모드에서 아이템 클릭 처리
        /// </summary>
        private void HandleShopModeClick(ItemData item, RectTransform slotTransform)
        {
            // 판매 가능한 아이템이면 판매 팝업 열기
            if (item.canSale && !item.questItem)
            {
                ShopManager.Instance?.OpenSellPopup(item, slotTransform);
            }
            else
            {
                Debug.Log($"{item.itemName}은(는) 판매할 수 없습니다.");
            }
        }

        #endregion

        /// <summary>
        /// InventoryItemUI 배열 반환 (마스크 제어용)
        /// </summary>
        public InventoryItemUI[] GetItemUISlots() => slots;

        // 인벤토리 팝업이 열려있는지 확인
        public bool IsInventoryPopupOpen()
        {
            return popupUI != null && popupUI.gameObject.activeSelf;
        }
    }
}