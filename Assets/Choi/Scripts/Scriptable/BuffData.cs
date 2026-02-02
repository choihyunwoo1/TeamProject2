using UnityEngine;
using Choi;

namespace hm
{
    [CreateAssetMenu(fileName = "BuffData", menuName = "Scriptable Objects/BuffData")]
    public class BuffData : ScriptableObject, ITooltipData
    {
        public TooltipType Type => TooltipType.Buff;

        [Header("ID")]
        public int id;

        [Header("Info")]
        public string buffName;

        [Header("Icon")]
        public Sprite icon;

        [Header("Buff Settings")]
        public float value;
        public bool isDebuff;               
        public float duration;
        public float tickInterval;
        public bool canRefresh;

        [Header("Tooltip")]
        [TextArea(2, 4)]
        public string description;
        public string subtitle;
    }
}