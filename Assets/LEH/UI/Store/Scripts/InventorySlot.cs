using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    public Image icon;
    public TMP_Text countText;

    private ShopItem item;
    private int count;

    public void SetItem(ShopItem newItem, int newCount)
    {
        item = newItem;
        count = newCount;

        icon.sprite = item.icon;
        countText.text = count > 1 ? count.ToString() : "";
        gameObject.SetActive(true);
    }

    public void Clear()
    {
        item = null;
        count = 0;
        gameObject.SetActive(false);
    }

    public void OnClick()
    {
        if (item == null) return;

        ShopActionMenu.Instance.Open(item, true, count); // true = 인벤
    }

    //툴팁
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null) return;

        ItemTooltip.Instance.Show(item, eventData.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ItemTooltip.Instance.Hide();
    }
}
