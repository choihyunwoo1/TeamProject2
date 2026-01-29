using UnityEngine;
namespace hm
{
    public enum ItemCategory { Weapon, UseItem, Material }
    public enum ItemType { Sword, Heal, Food, Material }

    [CreateAssetMenu(menuName = "Game/Item/ItemData")]
    public class ItemData : ScriptableObject, ITooltipData
    {
        public int id;
        public string devName;
        public string itemName;

        public ItemCategory category;
        public ItemType itemType;

        public bool stackable;
        public int maxStack;

        public bool usable;
        public bool consumable;

        public int priceBuy;
        public int priceSell;

        public bool canDrop;
        public bool canSale;
        public bool questItem;

        public Sprite icon;
        public TooltipType Type => TooltipType.Item;

        [Header("Tooltip")]
        public string subtitle;

        [TextArea(2, 5)]
        public string description;

        [TextArea(2, 5)]
        public string effectText;

        [TextArea(2, 5)]
        public string conditionText;
    }
}