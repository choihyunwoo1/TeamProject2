using TMPro;
using UnityEngine;


public class ShopManager : MonoBehaviour
{
    public ShopItem testItem;   // 임시 하나만
    public ShopSlot slot;
    public GameObject shopPanel;
    public TMP_Text goldText;

    private void Start()
    {
        slot.SetItem(testItem);
        RefreshGold();
    }

    public void Open()
    {
        shopPanel.SetActive(true);
        RefreshGold();
    }

    public void Close()
    {
        shopPanel.SetActive(false);
    }

    public void RefreshGold()
    {
        goldText.text = PlayerGold.Instance.gold.ToString();
    }
}
