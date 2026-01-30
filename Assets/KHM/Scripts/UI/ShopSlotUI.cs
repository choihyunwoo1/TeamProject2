using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace hm
{
    /// <summary>
    /// 상점의 개별 아이템 슬롯 UI
    /// </summary>
    public class ShopSlotUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Image iconImage;
        [SerializeField] private Button button;

        private ItemData itemData;
        private TooltipTrigger tooltipTrigger;
        private RectTransform rectTransform;

        private void Awake()
        {
            tooltipTrigger = GetComponent<TooltipTrigger>();
            rectTransform = GetComponent<RectTransform>();

            if (button == null)
                button = GetComponent<Button>();

            button.onClick.AddListener(OnSlotClicked);
        }

        /// <summary>
        /// 슬롯 초기화
        /// </summary>
        public void Initialize(ItemData data)
        {
            itemData = data;
            RefreshUI();
        }

        /// <summary>
        /// UI 새로고침
        /// </summary>
        public void RefreshUI()
        {
            if (itemData == null)
            {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);

            // 아이콘
            if (iconImage != null)
                iconImage.sprite = itemData.icon;

            // 툴팁 설정
            if (tooltipTrigger != null)
            {
                tooltipTrigger.SetData(itemData);
            }
        }

        /// <summary>
        /// 슬롯 클릭 시 구매 팝업 열기
        /// </summary>
        private void OnSlotClicked()
        {
            if (itemData == null) return;

            ShopManager.Instance?.OpenBuyPopup(itemData, rectTransform);
        }
    }
}