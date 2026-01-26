using UnityEngine;

namespace Choi
{
    public class BuffInstance
    {
        public BuffDataSO data;
        public float remainingTime;
        public int stack;

        public BuffInstance(BuffDataSO data)
        {
            this.data = data;
            this.remainingTime = data.duration;
            this.stack = 1;
        }

        public void Refresh()
        {
            remainingTime = data.duration;
        }

        public void AddStack()
        {
            stack = Mathf.Min(stack + 1, data.maxStack);
            remainingTime = data.duration;
        }
    }
}
