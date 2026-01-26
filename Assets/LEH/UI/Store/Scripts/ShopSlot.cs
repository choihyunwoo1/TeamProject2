using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    public Image icon;
    public ShopItem item;

    public void SetItem(ShopItem newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ItemTooltip.Instance.Show(item, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ItemTooltip.Instance.Hide();
    }
}
