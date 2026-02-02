using UnityEngine;

namespace hm
{
    public class QuickSlotUI : MonoBehaviour
    {
        [SerializeField] private QuickSlotSlotUI[] slots;

        public void EnterSelectMode()
        {
            foreach (var slot in slots)
                slot.SetSelectable(true);
        }

        public void ExitSelectMode()
        {
            foreach (var slot in slots)
                slot.SetSelectable(false);
        }

        //슬롯 비우기
        public void ClearSlot(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= slots.Length) return;
            slots[slotIndex].Clear();
        }

        //슬롯 세팅
        public void SetSlot(int slotIndex, ItemData item)
        {
            if (slotIndex < 0 || slotIndex >= slots.Length) return;
            slots[slotIndex].SetItem(item);
        }
    }
}
