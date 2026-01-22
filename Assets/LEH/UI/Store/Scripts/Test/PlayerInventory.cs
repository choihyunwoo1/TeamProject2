using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;
    public List<ShopItem> items = new();

    private void Awake()
    {
        Instance = this;
    }

    public void AddItem(ShopItem item)
    {
        items.Add(item);
        Debug.Log("획득: " + item.itemName);
    }
}
