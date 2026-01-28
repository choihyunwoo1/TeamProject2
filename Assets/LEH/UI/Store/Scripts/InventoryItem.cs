[System.Serializable]
public class InventoryItem
{
    public ShopItemData itemData;
    public int quantity;

    public InventoryItem(ShopItemData data, int amount)
    {
        itemData = data;
        quantity = amount;
    }
}
