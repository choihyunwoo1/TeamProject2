using UnityEngine;

namespace hm
{
    /// <summary>
    /// 상점에서 판매할 아이템 정보
    /// ItemData를 참조하여 가격과 재고 정보를 가짐
    /// </summary>
    [CreateAssetMenu(menuName = "Game/Shop/ShopItemData")]
    public class ShopItemData : ScriptableObject
    {
        [Header("아이템 참조")]
        public ItemData itemData;           // 실제 아이템 데이터

        [Header("상점 정보")]
        public int stockCount = -1;         // 재고 수량 (-1이면 무제한)
        public bool isUnlimited = true;     // 무제한 재고 여부

        // 구매 가격 (ItemData의 priceBuy 사용)
        public int BuyPrice => itemData != null ? itemData.priceBuy : 0;

        // 판매 가격 (ItemData의 priceSell 사용)
        public int SellPrice => itemData != null ? itemData.priceSell : 0;

        // 구매 가능 여부
        public bool CanBuy => isUnlimited || stockCount > 0;

        // 재고 감소
        public void DecreaseStock(int amount)
        {
            if (!isUnlimited)
            {
                stockCount = Mathf.Max(0, stockCount - amount);
            }
        }

        // 재고 증가 
        public void IncreaseStock(int amount)
        {
            if (!isUnlimited)
            {
                stockCount += amount;
            }
        }
    }
}