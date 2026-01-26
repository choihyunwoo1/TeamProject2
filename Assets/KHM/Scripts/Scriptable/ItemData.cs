using UnityEngine;
namespace Choi
{
    public enum ItemType
    {
        Consumable,     //소비 아이템
        Equipment,      //장비 아이템
        Quest           //퀘스트 아이템
    }

    [CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
    public class ItemData : ScriptableObject, ITooltipData
    {
        [Header("Tooltip Type")]
        public TooltipType Type => TooltipType.Item;

        [Header("Info")]
        public string itemName;

        [TextArea(2, 5)]
        public string description;

        public ItemType itemType;

        [Header("Icon")]
        public Sprite icon;
    }
}