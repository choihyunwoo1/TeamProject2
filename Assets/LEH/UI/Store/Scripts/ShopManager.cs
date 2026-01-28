using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public PlayerInventory inventory;
    public PlayerGold playerGold;
    public QuantityPopup quantityPopup;

    public void TryBuy(ShopItemData item)
    {
        if (item == null) return;

        quantityPopup.Open(99, amount =>
        {
            int cost = item.price * amount;
            if (!playerGold.SpendGold(cost)) return;

            inventory.AddItem(item, amount);
        });
    }

    public void TrySell(ShopItemData item)
    {
        quantityPopup.Open(99, amount =>
        {
            if (!inventory.RemoveItem(item, amount)) return;

            playerGold.AddGold(item.price * amount);
        });
    }
}
