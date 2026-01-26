using TMPro;
using UnityEngine;

public class ShopActionMenu : MonoBehaviour
{
    public static ShopActionMenu Instance;

    public GameObject panel;
    public TMP_Text titleText;

    private ShopItem currentItem;
    private bool isInventory;
    private int itemCount;

    private void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void Open(ShopItem item, bool inventory, int count)
    {
        currentItem = item;
        isInventory = inventory;
        itemCount = count;

        titleText.text = item.itemName;
        panel.SetActive(true);
    }

    public void Buy()
    {
        int maxBuy = PlayerGold.Instance.gold / currentItem.price;
        if (maxBuy <= 0) return;

        QuantityPopup.Instance.Open(currentItem, false, maxBuy);
        panel.SetActive(false);
    }

    public void Sell()
    {
        QuantityPopup.Instance.Open(currentItem, true, itemCount);
        panel.SetActive(false);
    }

    public void Cancel()
    {
        panel.SetActive(false);
    }
}
