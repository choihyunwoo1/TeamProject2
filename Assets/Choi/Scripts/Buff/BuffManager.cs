using System.Collections.Generic;
using UnityEngine;

namespace Choi
{
    public class BuffManager : MonoBehaviour
    {
        private Dictionary<string, BuffInstance> activeBuffs = new Dictionary<string, BuffInstance>();
        private IBuffReceiver buffReceiver;

        private void Awake()
        {
            buffReceiver = GetComponent<IBuffReceiver>();
            if (buffReceiver == null)
            {
                Debug.LogError("BuffManager: IBuffReceiver를 구현한 컴포넌트가 필요합니다.");
            }
        }

        private void Update()
        {
            if (activeBuffs.Count == 0) return;

            List<string> expired = new List<string>();

            foreach (var pair in activeBuffs)
            {
                var buff = pair.Value;
                buff.remainingTime -= Time.deltaTime;

                if (buff.remainingTime <= 0f)
                    expired.Add(pair.Key);
            }

            foreach (var id in expired)
                RemoveBuff(id);
        }

        public void AddBuff(BuffDataSO data)
        {
            if (activeBuffs.TryGetValue(data.buffID, out var inst))
            {
                if (data.isStackable)
                {
                    inst.AddStack();
                }
                else
                {
                    inst.Refresh(); // 단일 버프는 시간만 리셋
                }
            }
            else
            {
                var newBuff = new BuffInstance(data);
                activeBuffs.Add(data.buffID, newBuff);
                buffReceiver.ApplyBuff(data, newBuff.stack);
            }
        }

        public void RemoveBuff(string buffID)
        {
            if (activeBuffs.TryGetValue(buffID, out var inst))
            {
                buffReceiver.RemoveBuff(inst.data);
                activeBuffs.Remove(buffID);
            }
        }
    }
}
