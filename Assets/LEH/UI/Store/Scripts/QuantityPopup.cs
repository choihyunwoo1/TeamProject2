using TMPro;
using UnityEngine;

public class QuantityPopup : MonoBehaviour
{
    public static QuantityPopup Instance;

    public GameObject panel;
    public TMP_Text titleText;
    public TMP_Text countText;
    public TMP_Text totalPriceText;

    private ShopItem currentItem;
    private bool isSelling;
    private int currentCount;
    private int maxCount;

    private void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void Open(ShopItem item, bool selling, int max)
    {
        currentItem = item;
        isSelling = selling;
        maxCount = Mathf.Max(1, max);
        currentCount = 1;

        titleText.text = selling
            ? $"판매 수량 선택"
            : $"구매 수량 선택";

        Refresh();
        panel.SetActive(true);
    }

    void Refresh()
    {
        countText.text = currentCount.ToString();

        int totalPrice = currentItem.price * currentCount;

        if (isSelling)
            totalPriceText.text = $"총 획득 : {totalPrice} G";
        else
            totalPriceText.text = $"총 가격 : {totalPrice} G";
    }

    public void Plus()
    {
        if (currentCount < maxCount)
            currentCount++;
        Refresh();
    }

    public void Minus()
    {
        if (currentCount > 1)
            currentCount--;
        Refresh();
    }

    public void Max()
    {
        currentCount = maxCount;
        Refresh();
    }

    public void Confirm()
    {
        if (isSelling)
        {
            // 판매
            PlayerInventory.Instance.RemoveItem(currentItem, currentCount);
            PlayerGold.Instance.AddGold(currentItem.price * currentCount);
        }
        else
        {
            // 구매
            PlayerInventory.Instance.AddItem(currentItem, currentCount);
            PlayerGold.Instance.SpendGold(currentItem.price * currentCount);
        }

        Close();
    }

    public void Cancel()
    {
        Close();
    }

    void Close()
    {
        panel.SetActive(false);
    }
}
