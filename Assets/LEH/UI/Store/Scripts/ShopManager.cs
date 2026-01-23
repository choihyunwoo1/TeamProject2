using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    [Header("Panels")]
    public GameObject shopPanel;

    [Header("Gold UI")]
    public TMP_Text goldText;

    private void Awake()
    {
        Instance = this;
        shopPanel.SetActive(false);
    }

    // ================== 상점 열기 ==================
    public void OpenShop()
    {
        shopPanel.SetActive(true);
        RefreshGold();
        Time.timeScale = 0f; // 조작 제한
    }

    // ShopNPC / 다른 스크립트 호환용
    public void Open()
    {
        OpenShop();
    }

    // ================== 상점 닫기 ==================
    public void CloseShop()
    {
        shopPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    // ================== 골드 ==================
    public void RefreshGold()
    {
        goldText.text = PlayerGold.Instance.gold.ToString();
    }

    // ================== 수량 패널 열기 ==================
    public void OpenQuantityPanel(ShopItem item, bool isBuy)
    {
        int max;

        if (isBuy)
        {
            max = PlayerGold.Instance.gold / item.price;
        }
        else
        {
            max = PlayerInventory.Instance.GetItemCount(item);
        }

        if (max <= 0) return;

        QuantityPopup.Instance.Open(item, isBuy, max);
    }

    // ================== 구매 ==================
    public void BuyItem(ShopItem item, int quantity)
    {
        int totalPrice = item.price * quantity;

        if (!PlayerGold.Instance.CanSpend(totalPrice))
            return;

        PlayerGold.Instance.SpendGold(totalPrice);
        PlayerInventory.Instance.AddItem(item, quantity);

        RefreshGold();
    }

    // ================== 판매 ==================
    public void SellItem(ShopItem item, int quantity)
    {
        if (!PlayerInventory.Instance.HasItem(item, quantity))
            return;

        PlayerInventory.Instance.RemoveItem(item, quantity);
        PlayerGold.Instance.gold += item.price * quantity;

        RefreshGold();
    }
}
