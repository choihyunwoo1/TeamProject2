using Choi;

namespace hm
{
    public enum InventorySlotState
    {
        Empty,
        Normal,
        Locked
    }

    public class InventorySlot
    {
        public ItemData item;
        public int count;
        public int locked;

        public bool IsEmpty => item == null || count <= 0;
        public int AvailableCount => count - locked;

        public InventorySlot(ItemData item, int count)
        {
            this.item = item;
            this.count = count;
            locked = 0;
        }

        public void Clear()
        {
            item = null;
            count = 0;
            locked = 0;
        }
    }
}
