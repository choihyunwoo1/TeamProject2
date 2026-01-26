using System.Collections.Generic;
using UnityEngine;

namespace hm
{

    public class BuffUIManager : MonoBehaviour
    {
        [SerializeField] private Transform buffContainer;
        [SerializeField] private BuffIconUI buffPrefab;

        private Dictionary<int, BuffIconUI> activeBuffs = new();

        public void AddBuff(BuffData data)
        {
            //이미 존재하는 버프일 경우
            if (activeBuffs.TryGetValue(data.id, out var existing))
            {
                // 중첩 / 지속시간 갱신 여부는 게임 로직에서 결정

                return;
            }

            var icon = Instantiate(buffPrefab, buffContainer);
            icon.SetData(data);
            activeBuffs[data.id] = icon;
        }

        public void RemoveBuff(int buffId)
        {
            if (!activeBuffs.TryGetValue(buffId, out var icon)) return;
            Destroy(icon.gameObject);
            activeBuffs.Remove(buffId);
        }
    }
}