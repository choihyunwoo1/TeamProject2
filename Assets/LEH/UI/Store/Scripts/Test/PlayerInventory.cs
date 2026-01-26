using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    public List<InventoryItem> items = new List<InventoryItem>();
    public int maxStack = 20;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    //아이템 보유 여부
    public bool HasItem(ShopItem item, int quantity)
    {
        InventoryItem found = items.Find(i => i.item == item);
        return found != null && found.quantity >= quantity;
    }

    //수량 얻기
    public int GetItemCount(ShopItem item)
    {
        InventoryItem found = items.Find(i => i.item == item);
        return found != null ? found.quantity : 0;
    }

    //아이템 추가
    public void AddItem(ShopItem item, int amount)
    {
        InventoryItem found = items.Find(i => i.item == item);

        if (found != null)
        {
            found.quantity = Mathf.Min(found.quantity + amount, maxStack);
        }
        else
        {
            items.Add(new InventoryItem(item, Mathf.Min(amount, maxStack)));
        }
    }

    //아이템 제거
    public void RemoveItem(ShopItem item, int amount)
    {
        InventoryItem found = items.Find(i => i.item == item);
        if (found == null) return;

        found.quantity -= amount;

        if (found.quantity <= 0)
            items.Remove(found);
    }
}
