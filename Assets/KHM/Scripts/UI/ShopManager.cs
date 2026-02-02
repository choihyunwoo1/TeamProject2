using UnityEngine;
using TMPro;
using System.Collections.Generic;

namespace hm
{
    /// <summary>
    /// 상점 시스템을 관리하는 매니저
    /// 구매/판매 로직 및 골드 관리
    /// </summary>
    public class ShopManager : MonoBehaviour
    {
        public static ShopManager Instance { get; private set; }

        [Header("Popup References")]
        [SerializeField] private ShopActionPopup actionPopup;       // 구매/판매 선택 팝업
        [SerializeField] private ShopQuantityPopup quantityPopup;   // 수량 조절 팝업

        [Header("Shop Items")]
        [SerializeField] private List<ItemData> shopItems;          // 상점에서 판매하는 아이템 목록 (ItemData 직접 사용)

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            // Inventory 초기화 대기 후 골드 초기화
            StartCoroutine(InitializeAfterInventory());
        }

        private System.Collections.IEnumerator InitializeAfterInventory()
        {
            // Inventory가 초기화될 때까지 대기
            while (Inventory.Instance == null)
            {
                yield return null;
            }
        }

        #region Shop Actions

        /// <summary>
        /// 아이템 구매 시도 (상점 슬롯 클릭)
        /// </summary>
        public void OpenBuyPopup(ItemData item, RectTransform slotTransform)
        {
            if (item == null) return;

            // ⭐️ ShopQuantityPopup이 열려있으면 무시
            if (IsQuantityPopupOpen())
            {
                Debug.Log("[ShopManager] 수량 조절 팝업이 열려있어 다른 팝업을 열 수 없습니다.");
                return;
            }

            // 구매 확인 팝업 열기
            actionPopup.OpenForBuy(item, slotTransform, OnBuyConfirmed);
        }

        /// <summary>
        /// 아이템 판매 시도 (인벤토리에서 클릭)
        /// </summary>
        public void OpenSellPopup(ItemData item, RectTransform slotTransform)
        {
            if (item == null) return;

            // ⭐️ ShopQuantityPopup이 열려있으면 무시
            if (IsQuantityPopupOpen())
            {
                Debug.Log("[ShopManager] 수량 조절 팝업이 열려있어 다른 팝업을 열 수 없습니다.");
                return;
            }

            // 판매 불가능한 아이템 체크
            if (!item.canSale || item.questItem)
            {
                Debug.Log($"[ShopManager] {item.itemName}은(는) 판매할 수 없습니다.");
                return;
            }

            // 보유 수량 체크
            int availableCount = Inventory.Instance.GetItemCount(item);
            if (availableCount <= 0)
            {
                Debug.Log($"[ShopManager] {item.itemName}을(를) 보유하고 있지 않습니다.");
                return;
            }

            // 판매 확인 팝업 열기
            actionPopup.OpenForSell(item, slotTransform, OnSellConfirmed);
        }

        /// <summary>
        /// 구매 확정 후 수량 팝업 열기
        /// </summary>
        private void OnBuyConfirmed(ItemData item)
        {
            int maxBuyable = CalculateMaxBuyableAmount(item);

            if (maxBuyable <= 0)
            {
                Debug.Log($"[ShopManager] 골드가 부족합니다.");
                return;
            }

            // 수량 조절 팝업 열기
            quantityPopup.OpenForBuy(item, maxBuyable, ExecuteBuy);
        }

        /// <summary>
        /// 판매 확정 후 수량 팝업 열기
        /// </summary>
        private void OnSellConfirmed(ItemData item)
        {
            int maxSellable = Inventory.Instance.GetItemCount(item);

            if (maxSellable <= 0)
            {
                Debug.Log($"[ShopManager] 판매할 수 없습니다.");
                return;
            }

            // 수량 조절 팝업 열기
            quantityPopup.OpenForSell(item, maxSellable, ExecuteSell);
        }

        /// <summary>
        /// 실제 구매 실행
        /// </summary>
        private void ExecuteBuy(ItemData item, int amount)
        {
            if (item == null) return;

            int totalCost = item.priceBuy * amount;

            if (Inventory.Instance.SpendGold(totalCost))
            {
                Inventory.Instance.Add(item, amount);
            }
            else
            {
                Debug.Log("골드 부족");
            }

            Debug.Log($"[ShopManager] 구매 완료 - {item.itemName} x{amount}, 비용: {totalCost}G");
        }

        /// <summary>
        /// 실제 판매 실행
        /// </summary>
        private void ExecuteSell(ItemData item, int amount)
        {
            if (item == null) return;

            // 아이템 제거
            Inventory.Instance.Remove(item, amount);

            // 골드 추가
            int totalPrice = item.priceSell * amount;
            Inventory.Instance.AddGold(totalPrice);

            Debug.Log($"[ShopManager] 판매 완료 - {item.itemName} x{amount}, 획득: {totalPrice}G");
        }

        /// <summary>
        /// 최대 구매 가능 수량 계산 (골드 기준)
        /// </summary>
        private int CalculateMaxBuyableAmount(ItemData item)
        {
            if (item == null || item.priceBuy <= 0) return 0;

            return Inventory.Instance.Gold / item.priceBuy;
        }

        /// <summary>
        /// ⭐️ ShopQuantityPopup이 열려있는지 확인
        /// </summary>
        public bool IsQuantityPopupOpen()
        {
            return quantityPopup != null && quantityPopup.gameObject.activeSelf;
        }

        #endregion

        #region Public Getters

        /// <summary>
        /// 상점 아이템 목록 반환
        /// </summary>
        public List<ItemData> GetShopItems() => shopItems;

        #endregion
    }
}