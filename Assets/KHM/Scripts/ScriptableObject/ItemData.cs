using UnityEngine;
namespace hm
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
    public class ItemData : ScriptableObject, ITooltipData
    {
        [Header("Tooltip Type")]
        public TooltipType Type => TooltipType.Item;

        [Header("Info")]
        public string itemName;

        [TextArea(2, 5)]
        public string description;

        [Header("Icon")]
        public Sprite icon;
    }
}