using UnityEngine;
using UnityEngine.EventSystems;

public class ShopSlot : MonoBehaviour, IPointerClickHandler
{
    public ShopItem item;
    public ShopActionMenu actionMenu;
    public ShopManager shopManager;

    public void OnPointerClick(PointerEventData eventData)
    {
        actionMenu.Open(item.data, shopManager);
    }
}
