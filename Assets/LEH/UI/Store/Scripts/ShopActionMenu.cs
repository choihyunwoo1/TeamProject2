using UnityEngine;

public class ShopActionMenu : MonoBehaviour
{
    ShopItemData currentItem;
    ShopManager shopManager;

    public void Open(ShopItemData item, ShopManager manager)
    {
        currentItem = item;
        shopManager = manager;
        gameObject.SetActive(true);
    }

    public void Buy()
    {
        shopManager.TryBuy(currentItem);
        gameObject.SetActive(false);
    }

    public void Sell()
    {
        shopManager.TrySell(currentItem);
        gameObject.SetActive(false);
    }
}
