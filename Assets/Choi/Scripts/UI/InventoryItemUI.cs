using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace hm
{
    public class InventoryItemUI : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI countText;
        [SerializeField] private Image mask; // 잠금 표시용 마스크
        [SerializeField] private Button button; // 버튼 컴포넌트
        [SerializeField] private Image selectImage;

        private ItemData itemData;
        private TooltipTrigger tooltipTrigger;
        private RectTransform rectTransform;
        private InventorySlot currentSlot;

        public ItemData ItemData => itemData;

        private void Awake()
        {
            tooltipTrigger = GetComponent<TooltipTrigger>();
            rectTransform = GetComponent<RectTransform>();

            // Button 컴포넌트가 없으면 자동으로 가져오기
            if (button == null)
                button = GetComponent<Button>();
        }

        public void SetSlot(InventorySlot slot)
        {
            currentSlot = slot;

            if (slot == null || slot.IsEmpty)
            {
                Clear();
                return;
            }

            iconImage.gameObject.SetActive(true);
            countText.gameObject.SetActive(true);

            itemData = slot.item;
            iconImage.sprite = slot.item.icon;

            countText.text = slot.count > 1 ? slot.count.ToString() : "";

            if (tooltipTrigger != null)
                tooltipTrigger.SetData(slot.item);

            UpdateButtonState();
        }

        public void Clear()
        {
            currentSlot = null;
            itemData = null;
            iconImage.sprite = null;
            iconImage.gameObject.SetActive(false);
            countText.gameObject.SetActive(false);

            if (tooltipTrigger != null)
                tooltipTrigger.ClearData();

            mask.enabled = false;
            selectImage.enabled = false;

            // 버튼 비활성화
            if (button != null)
                button.interactable = false;
        }

        /// <summary>
        /// 슬롯 바인딩 및 즉시 UI 갱신
        /// </summary>
        public void Bind(InventorySlot slot)
        {
            currentSlot = slot;
            UpdateVisual();
        }

        /// <summary>
        /// 실제 UI 갱신
        /// </summary>
        private void UpdateVisual()
        {
            // 슬롯이 비어있거나 count가 0 이하면 Clear
            if (currentSlot == null || currentSlot.IsEmpty)
            {
                Clear();
                return;
            }

            itemData = currentSlot.item;

            iconImage.gameObject.SetActive(true);
            countText.gameObject.SetActive(true);

            iconImage.sprite = itemData.icon;

            // 수량 텍스트 갱신 - 실시간으로 currentSlot 참조
            countText.text = currentSlot.count > 1 ? currentSlot.count.ToString() : "";

            if (tooltipTrigger != null)
                tooltipTrigger.SetData(itemData);

            UpdateButtonState();
        }

        /// <summary>
        /// 현재 인벤토리 모드에 따라 버튼 상태 업데이트
        /// </summary>
        private void UpdateButtonState()
        {
            if (button == null || itemData == null) return;
            if (currentSlot == null) return;
            if (InventoryUI.Instance == null) return;

            InventoryMode currentMode = InventoryUI.Instance.GetCurrentMode();

            bool showMask = false;
            bool interactable = true;

            switch (currentMode)
            {
                case InventoryMode.Normal:
                    // 일반 모드: 잠긴 아이템만 마스크
                    interactable = true;
                    showMask = currentSlot.locked > 0;
                    break;

                case InventoryMode.WeaponUpgrade:
                    // 무기개조 모드
                    bool isMaterial = itemData.itemType == ItemType.Material;
                    interactable = isMaterial;

                    if (isMaterial)
                    {
                        // Material: 삽입된 경우에만 마스크
                        showMask = currentSlot.locked > 0;
                    }
                    else
                    {
                        // Material 아닌 것: 항상 마스크
                        showMask = true;
                    }
                    break;

                case InventoryMode.Shop:
                    // 상점 모드
                    bool canSell = itemData.canSale && !itemData.questItem;
                    interactable = canSell;
                    showMask = !canSell;
                    break;
            }

            button.interactable = interactable;
            mask.enabled = showMask;
        }

        /// <summary>
        /// 슬롯 클릭 시 호출
        /// </summary>
        public void OnClick()
        {
            if (itemData == null) return;

            // ⭐️ 모든 모드에서 selectImage 표시
            if (InventoryUI.Instance != null)
            {
                InventoryUI.Instance.SetSelectedSlot(this);
                InventoryUI.Instance.OnItemClicked(itemData, rectTransform);
            }
        }

        /// <summary>
        /// 마스크 직접 제어 (외부에서 호출)
        /// </summary>
        public void SetMask(bool enabled)
        {
            if (mask != null)
                mask.enabled = enabled;
        }

        /// <summary>
        /// 선택 이미지 제어
        /// </summary>
        public void SetSelected(bool selected)
        {
            if (selectImage != null)
                selectImage.enabled = selected;
        }

        /// <summary>
        /// 버튼 상태 강제 갱신 (외부에서 호출)
        /// </summary>
        public void RefreshButtonState()
        {
            UpdateVisual();
        }
    }
}