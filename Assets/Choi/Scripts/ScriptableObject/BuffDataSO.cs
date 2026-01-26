using UnityEngine;

namespace Choi
{
    [CreateAssetMenu(menuName = "GameData/Buff", fileName = "NewBuff")]
    public class BuffDataSO : ScriptableObject
    {
        [Header("Identification")]
        public string buffID;                // 고유 ID
        public string buffName;              // 버프 표시 이름
        public string description;

        [Header("Duration")]
        public float duration = 5f;          // 지속 시간
        public bool isStackable = false;     // 중첩 여부
        public int maxStack = 3;

        [Header("Effect Values")]
        public float value = 10f;            // 버프 효과 수치(공격력 증가, 이동속도 증가 등)

        [Header("Effect Type")]
        public BuffEffectType effectType;    // 효과 종류
    }

    public enum BuffEffectType
    {
        IncreaseAttack,
        IncreaseDefense,
        IncreaseMoveSpeed,
        RegenHP,
        Custom
    }
}
