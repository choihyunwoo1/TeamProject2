using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace hm
{
    /// <summary>
    /// 구매/판매 선택 팝업
    /// 구매하기/판매하기/선택취소 버튼 표시
    /// </summary>
    public class ShopActionPopup : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button actionButton;           // 구매하기 or 판매하기
        [SerializeField] private Button cancelButton;           // 선택취소
        [SerializeField] private TextMeshProUGUI actionButtonText;

        private ItemData currentItem;
        private Action<ItemData> onBuyConfirmed;
        private Action<ItemData> onSellConfirmed;
        private bool isBuyMode;

        private RectTransform popupRect;      // 내 팝업
        private RectTransform slotRect;       // 기준이 되는 슬롯

        private void Awake()
        {
            // 버튼 이벤트 연결
            actionButton.onClick.AddListener(OnActionButtonClicked);
            cancelButton.onClick.AddListener(OnCancelButtonClicked);

            popupRect = GetComponent<RectTransform>();
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 구매 모드로 팝업 열기
        /// </summary>
        public void OpenForBuy(ItemData item, RectTransform slot, Action<ItemData> onConfirmed)
        {
            currentItem = item;
            slotRect = slot;
            onBuyConfirmed = onConfirmed;
            isBuyMode = true;

            actionButtonText.text = "구매하기";

            gameObject.SetActive(true);
            SetPositionRelativeToSlot(slotRect);
        }

        /// <summary>
        /// 판매 모드로 팝업 열기
        /// </summary>
        public void OpenForSell(ItemData item, RectTransform slot, Action<ItemData> onConfirmed)
        {
            if (item == null) return;

            // 팝업 열기 전 인벤토리 선택 상태 초기화
            if (InventoryUI.Instance != null)
            {
                InventoryUI.Instance.ClearSelection();
            }

            currentItem = item;
            slotRect = slot;
            onSellConfirmed = onConfirmed;
            onBuyConfirmed = null;
            isBuyMode = false;

            actionButtonText.text = "판매하기";

            gameObject.SetActive(true);
            SetPositionRelativeToSlot(slotRect);
        }

        /// <summary>
        /// 구매/판매 버튼 클릭
        /// </summary>
        private void OnActionButtonClicked()
        {
            // ⭐️ ShopQuantityPopup이 이미 열려있으면 무시
            if (ShopManager.Instance != null && ShopManager.Instance.IsQuantityPopupOpen())
            {
                Debug.Log("[ShopActionPopup] 수량 조절 팝업이 이미 열려있습니다.");
                return;
            }

            if (isBuyMode)
                onBuyConfirmed?.Invoke(currentItem);
            else
                onSellConfirmed?.Invoke(currentItem);

            gameObject.SetActive(false);
        }

        /// <summary>
        /// 취소 버튼 클릭
        /// </summary>
        private void OnCancelButtonClicked()
        {
            gameObject.SetActive(false);
        }

        //팝업 위치 오른쪽으로 70픽셀 옆에 띄우기
        private void SetPositionRelativeToSlot(RectTransform slotTransform)
        {
            if (slotTransform == null || popupRect == null) return;

            Vector3 slotWorldPos = slotTransform.position;
            Vector3 popupWorldPos = slotWorldPos + new Vector3(70f, 0f, 0f);
            popupRect.position = popupWorldPos;
        }
    }
}