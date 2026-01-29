using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<InventoryItem> items = new();

    public void AddItem(ShopItemData data, int amount)
    {
        var item = items.Find(i => i.itemData == data);
        if (item != null)
            item.quantity += amount;
        else
            items.Add(new InventoryItem(data, amount));
    }

    public bool RemoveItem(ShopItemData data, int amount)
    {
        var item = items.Find(i => i.itemData == data);
        if (item == null || item.quantity < amount) return false;

        item.quantity -= amount;
        if (item.quantity <= 0)
            items.Remove(item);

        return true;
    }
}
