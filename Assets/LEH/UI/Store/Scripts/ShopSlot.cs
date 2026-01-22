using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    public Image icon;
    private ShopItem item;

    public void SetItem(ShopItem newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
    }

    public void Buy()
    {
        if (PlayerGold.Instance.gold < item.price)
        {
            Debug.Log("골드 부족");
            return;
        }

        PlayerGold.Instance.gold -= item.price;
        PlayerInventory.Instance.AddItem(item);
    }
}
