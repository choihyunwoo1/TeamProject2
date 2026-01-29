using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ShopSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Item Data")]
    public ShopItemData data;              // 아이템 데이터 (ScriptableObject)

    [Header("UI")]
    public Image iconImage;                // 슬롯 아이콘
    public TMP_Text nameText;              // 아이템 이름
    public TMP_Text priceText;             // 가격 텍스트

    [Header("Systems")]
    public ShopActionMenu actionMenu;
    public ShopManager shopManager;
    public ItemTooltip tooltip;

    private void Start()
    {
        RefreshUI();
    }

    // 아이템 UI 갱신
    public void RefreshUI()
    {
        if (data == null) return;

        if (iconImage != null)
            iconImage.sprite = data.icon;

        if (nameText != null)
            nameText.text = data.itemName;

        if (priceText != null)
            priceText.text = data.price.ToString() + " G";
    }

    // 클릭 구매/판매 메뉴
    public void OnPointerClick(PointerEventData eventData)
    {
        if (data == null) return;

        actionMenu.Open(data, shopManager);
    }

    // 마우스 올리면 툴팁
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (data == null || tooltip == null) return;

        tooltip.Show(data);
    }

    // 마우스 나가면 툴팁 끄기
    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltip == null) return;

        tooltip.Hide();
    }
}
