using UnityEngine;

[System.Serializable]
public class ShopItem
{
    public string itemName;
    public Sprite icon;
    public int price;
    [TextArea]
    public string description; // ← 이거 하나만
}
