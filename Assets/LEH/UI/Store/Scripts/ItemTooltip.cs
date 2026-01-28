using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour
{
    public Text nameText;
    public Text descriptionText;
    public Text priceText;

    public void Show(ShopItemData data)
    {
        gameObject.SetActive(true);
        nameText.text = data.itemName;
        descriptionText.text = data.description;
        priceText.text = data.price.ToString();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
