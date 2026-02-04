using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace hm
{
    /// <summary>
    /// 구매/판매 수량 조절 팝업
    /// +/- 버튼으로 수량 조절, 가격 실시간 표시
    /// </summary>
    public class ShopQuantityPopup : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI quantityText;
        [SerializeField] private Button increaseButton;        // + 버튼
        [SerializeField] private Button decreaseButton;        // - 버튼
        [SerializeField] private Button confirmButton;         // 구매하기/판매하기
        [SerializeField] private Button cancelButton;          // 선택취소
        [SerializeField] private TextMeshProUGUI confirmButtonText;

        // ⭐️ 불필요한 변수 제거: fixedUnitPrice만 사용
        private int unitPrice;                  // 개당 가격 (구매가 또는 판매가)
        private int currentQuantity;            // 현재 선택 수량
        private int maxQuantity;                // 최대 수량
        private bool isBuyMode;                 // 구매/판매 모드

        private ItemData currentItem;

        private Action<ItemData, int> onBuyConfirmed;
        private Action<ItemData, int> onSellConfirmed;

        private void Awake()
        {
            // 버튼 이벤트 연결
            increaseButton.onClick.AddListener(OnIncreaseClicked);
            decreaseButton.onClick.AddListener(OnDecreaseClicked);
            confirmButton.onClick.AddListener(OnConfirmClicked);
            cancelButton.onClick.AddListener(OnCancelClicked);

            gameObject.SetActive(false);
        }

        /// <summary>
        /// 구매 모드로 팝업 열기
        /// </summary>
        public void OpenForBuy(ItemData item, int maxAmount, Action<ItemData, int> onConfirmed)
        {
            if (item == null) return;

            currentItem = item;
            onBuyConfirmed = onConfirmed;
            onSellConfirmed = null;
            isBuyMode = true;

            currentQuantity = 1;
            maxQuantity = maxAmount;
            unitPrice = item.priceBuy;

            titleText.text = $"{item.itemName} 구매";
            confirmButtonText.text = "구매하기";

            UpdateUI();
            gameObject.SetActive(true);
        }

        /// <summary>
        /// 판매 모드로 팝업 열기
        /// </summary>
        public void OpenForSell(ItemData item, int maxAmount, Action<ItemData, int> onConfirmed)
        {
            if (item == null) return;

            currentItem = item;
            onSellConfirmed = onConfirmed;
            onBuyConfirmed = null;
            isBuyMode = false;

            currentQuantity = 1;
            maxQuantity = maxAmount;
            unitPrice = item.priceSell;

            titleText.text = $"{item.itemName} 판매";
            confirmButtonText.text = "판매하기";

            UpdateUI();
            gameObject.SetActive(true);
        }

        /// <summary>
        /// + 버튼 클릭
        /// </summary>
        private void OnIncreaseClicked()
        {
            if (currentQuantity < maxQuantity)
            {
                currentQuantity++;
                UpdateUI();
            }
        }

        /// <summary>
        /// - 버튼 클릭
        /// </summary>
        private void OnDecreaseClicked()
        {
            if (currentQuantity > 1)
            {
                currentQuantity--;
                UpdateUI();
            }
        }

        /// <summary>
        /// 확정 버튼 클릭
        /// </summary>
        private void OnConfirmClicked()
        {
            if (isBuyMode)
            {
                onBuyConfirmed?.Invoke(currentItem, currentQuantity);
            }
            else
            {
                onSellConfirmed?.Invoke(currentItem, currentQuantity);
            }

            gameObject.SetActive(false);
        }

        /// <summary>
        /// 취소 버튼 클릭
        /// </summary>
        private void OnCancelClicked()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// UI 업데이트 (수량 및 가격)
        /// </summary>
        private void UpdateUI()
        {
            quantityText.text = currentQuantity.ToString();

            int totalPrice = unitPrice * currentQuantity;

            if (isBuyMode)
            {
                confirmButtonText.text = $"구매하기 ({totalPrice:N0} G)";
            }
            else
            {
                confirmButtonText.text = $"판매하기 ({totalPrice:N0} G)";
            }

            // 버튼 활성화/비활성화
            decreaseButton.interactable = currentQuantity > 1;
            increaseButton.interactable = currentQuantity < maxQuantity;
        }
    }
}