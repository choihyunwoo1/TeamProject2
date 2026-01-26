[System.Serializable]
public class InventoryItem
{
    public ShopItem item;
    public int quantity;

    public InventoryItem(ShopItem item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }
}
