using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Item Data")]
public class ShopItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public int price;              // 골드 가격
    [TextArea] public string description;
}
