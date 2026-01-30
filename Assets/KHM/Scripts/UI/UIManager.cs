using System.Collections.Generic;
using UnityEngine;

namespace hm
{
    /// <summary>
    /// UI를 관리하는 매니저
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }
        [SerializeField] private Transform uiCanvasRoot;

        //툴팁
        [SerializeField] private TooltipController tooltipController;

        //팝업
        [Header("Popup")]
        [SerializeField] private List<PopupUIBase> popups;
        private PopupUIBase Get(PopupType type) => popupMap[type];

        private Dictionary<PopupType, PopupUIBase> popupMap;

        //퀵슬롯
        [SerializeField] private QuickSlotUI quickSlotUI;
        private ItemData selectedItem;                          //퀵슬롯에 넣을 아이템
        private Dictionary<int, ItemData> slotToItem = new();   // 현재 퀵슬롯에 등록된 아이템들
        private Dictionary<ItemData, int> itemToSlot = new();   // 아이템이 어느 슬롯에 있는지 역으로 찾기용

        //스킬
        [SerializeField] private SkillUI skillUI;

        private void Awake()
        {
            Instance = this;

            popupMap = new Dictionary<PopupType, PopupUIBase>();
            foreach (var popup in popups)
            {
                if (!popupMap.ContainsKey(popup.Type))
                    popupMap.Add(popup.Type, popup);

                popup.gameObject.SetActive(false);
            }
        }

        #region Tooltip
        //툴팁 보여주기
        public void ShowTooltip(ITooltipData data)
        {
            tooltipController.Show(data);
        }

        //툴팁 숨기기
        public void HideTooltip()
        {
            tooltipController.HideAll();
        }

        #endregion

        #region Popup
        public bool AnyPopupOpen()
        {
            foreach (var popup in popupMap.Values)
            {
                if (popup.IsOpen)
                    return true;
            }
            return false;
        }

        //I : 인벤토리
        //설정이 열려 있으면 무시
        public void HandleInventory()
        {
            var inventory = Get(PopupType.Inventory);
            var settings = Get(PopupType.Setting);

            if (settings.IsOpen)
                return;

            if (inventory.IsOpen)
                inventory.Hide();
            else
                inventory.Show();
        }

        //M : 지도
        //열 때 인벤이 열려 있으면 인벤 닫기, 설정 열려 있으면 무시
        public void HandleMap()
        {
            var map = Get(PopupType.Map);
            var inventory = Get(PopupType.Inventory);
            var settings = Get(PopupType.Setting);

            if (settings.IsOpen)
                return;

            if (map.IsOpen)
            {
                map.Hide();
            }
            else
            {
                inventory.Hide();
                map.Show();
            }
        }

        //ESC
        //팝업 하나라도 열려 있으면 하나 닫기, 아무 것도 없으면 설정 열기
        public void HandleEscape()
        {
            var settings = Get(PopupType.Setting);

            if (AnyPopupOpen())
            {
                CloseAllPopups();
            }
            else
            {
                settings.Show();
            }
        }
    
        public void TogglePopup(PopupType type)
        {
            if (!popupMap.TryGetValue(type, out var popup)) return;

            bool isOpen = popup.gameObject.activeSelf;
            CloseAllPopups();

            if (!isOpen)
                popup.Show();
        }
        public void CloseAllPopups()
        {
            foreach (var popup in popupMap.Values)
                popup.Hide();
        }

        //마우스 버튼 클릭으로 팝업창 열기
        public void OpenInventory()
        {
            HandleInventory();
        }

        public void OpenMap()
        {
            HandleMap();
        }

        public void OpenSetting()
        {
            HandleEscape();
        }

        #endregion

        #region QuickSlot
        //인벤토리 아이템 클릭 시 호출
        public void SelectItemForQuickSlot(ItemData item)
        {
            // 소비 아이템만 허용
            if (item.category != ItemCategory.UseItem)
                return;

            selectedItem = item;
            quickSlotUI.EnterSelectMode();
        }


        //슬롯 클릭 시 호출
        public void AssignItemToSlot(int slotIndex)
        {
            if (selectedItem == null) return;

            // 이미 이 아이템이 다른 슬롯에 있다면
            if (itemToSlot.TryGetValue(selectedItem, out int prevSlot))
            {
                // 이전 슬롯 비우기
                slotToItem.Remove(prevSlot);
                quickSlotUI.ClearSlot(prevSlot);
                itemToSlot.Remove(selectedItem);
            }

            //현재 슬롯에 다른 아이템이 있다면?
            if (slotToItem.TryGetValue(slotIndex, out ItemData existingItem))
            {
                itemToSlot.Remove(existingItem);
            }

            //새로 등록
            slotToItem[slotIndex] = selectedItem;
            itemToSlot[selectedItem] = slotIndex;

            quickSlotUI.SetSlot(slotIndex, selectedItem);

            //마무리
            selectedItem = null;
            quickSlotUI.ExitSelectMode();
        }
        #endregion

        //무기에 따른 스킬셋을 불러오기
        public void ChangeSkillSet(SkillSetData skillSet)
        {
            skillUI.SetSkillSet(skillSet);
        }
    }
}