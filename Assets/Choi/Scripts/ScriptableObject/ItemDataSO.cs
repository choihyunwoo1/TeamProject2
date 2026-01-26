using UnityEngine;

namespace Choi
{
    [CreateAssetMenu(menuName = "GameData/Item")]
    public class ItemDataSO : ScriptableObject
    {
        public string itemName;
        public Sprite icon;                // UI 아이콘 (없으면 null 가능)
        public GameObject worldPrefab;     // 월드에서 떨어지는 실제 프리팹(나중에 채우면 됨)

        [Header("Optional Buff")]
        public BuffData buffToApply;       // 이 아이템이 적용하는 버프 (없어도 됨)

        [Header("Value")]
        public int amount = 1;             // 소모 아이템 수량 등
    }
}
