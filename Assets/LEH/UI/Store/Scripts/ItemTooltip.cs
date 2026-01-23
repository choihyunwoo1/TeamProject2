using TMPro;
using UnityEngine;

public class ItemTooltip : MonoBehaviour
{
    public static ItemTooltip Instance;

    public GameObject panel;
    public TMP_Text nameText;
    public TMP_Text descriptionText;

    private void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void Show(ShopItem item, Vector3 position)
    {
        nameText.text = item.itemName;
        descriptionText.text = item.description;

        panel.transform.position = position;
        panel.SetActive(true);
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}
