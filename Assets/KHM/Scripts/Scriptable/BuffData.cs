using UnityEngine;

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
        [TextArea(2, 4)]
        public string description;

        [Header("Icon")]
        public Sprite icon;

        [Header("Buff Settings")]
        public bool isDebuff;               //디버프 여부
        public bool isStackable;            //중첩 여부
        public int maxStack = 1;            //최대 중첩 수

        [Header("Duration")]
        public bool hasDuration = true;     // 지속 시간이 있는 버프인지 여부
        public float duration = 5f;         // 기본 지속 시간
    }
}