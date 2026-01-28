using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public ShopItemData data;

    public Image iconImage;
    public Text nameText;
    public Text priceText;

    public void Init(ShopItemData itemData)
    {
        data = itemData;
        iconImage.sprite = data.icon;
        nameText.text = data.itemName;
        priceText.text = data.price.ToString();
    }
}
