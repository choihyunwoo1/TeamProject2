using Choi;

namespace hm
{
    public enum InventorySlotState
    {
        Normal,
        Disabled,     // 제작 불가
        Locked        // 개조에 사용 중
    }

    public class InventorySlot
    {
        public ItemData item;
        public int count;
        public InventorySlotState state;
    }
}