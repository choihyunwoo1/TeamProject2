using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Text quantityText;

    InventoryItem item;

    public void SetItem(InventoryItem newItem)
    {
        item = newItem;
        icon.sprite = item.itemData.icon;
        icon.enabled = true;

        quantityText.text = item.quantity > 1 ? item.quantity.ToString() : "";
    }

    public void Clear()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        quantityText.text = "";
    }

    public InventoryItem GetItem() => item;
}
