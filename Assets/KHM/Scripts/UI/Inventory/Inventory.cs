using System;
using System.Collections.Generic;
using UnityEngine;

namespace hm
{
    public class Inventory : MonoBehaviour, IInventory
    {
        public static Inventory Instance { get; private set; }

        [SerializeField] private ItemDatabase itemDatabase;
        [SerializeField] private int maxSlotCount = 30;

        private List<InventorySlot> slots = new();

        //골드
        private int gold;
        public int Gold => gold;
        public event Action<int> OnGoldChanged;

        //인벤토리 모드
        public event Action OnInventoryChanged;

        private void Awake()
        {
            itemDatabase.Init();

            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            for (int i = 0; i < maxSlotCount; i++)
                slots.Add(new InventorySlot(null, 0));
        }
        #region Money
        public void AddGold(int amount)
        {
            gold += amount;
            OnGoldChanged?.Invoke(Gold);
        }

        public bool SpendGold(int amount)
        {
            if (Gold < amount) return false;
            gold -= amount;
            OnGoldChanged?.Invoke(Gold);
            return true;
        }
        #endregion

        #region Add

        public void Add(ItemData item, int count)
        {
            int remain = count;

            // 1. 기존 스택 채우기
            foreach (var slot in slots)
            {
                if (slot.item == item &&
                    item.stackable &&
                    slot.count < item.maxStack)
                {
                    int add = Mathf.Min(item.maxStack - slot.count, remain);
                    slot.count += add;
                    remain -= add;

                    if (remain <= 0)
                        break;
                }
            }

            // 2. 빈 슬롯 사용
            foreach (var slot in slots)
            {
                if (slot.IsEmpty)
                {
                    int add = Mathf.Min(item.maxStack, remain);
                    slot.item = item;
                    slot.count = add;
                    remain -= add;

                    if (remain <= 0)
                        break;
                }
            }

            OnInventoryChanged?.Invoke();
        }

        #endregion

        #region Remove

        public void Remove(ItemData item, int count)
        {
            int remain = count;

            Debug.Log($"[Inventory.Remove] {item.itemName} {count}개 제거 시작");

            foreach (var slot in slots)
            {
                if (slot.item == item && remain > 0)
                {
                    Debug.Log($"[Remove] 슬롯 발견 - count: {slot.count}, locked: {slot.locked}, AvailableCount: {slot.AvailableCount}");

                    // 제거 가능한 수량 = 전체 count와 남은 수량 중 작은 값
                    // locked는 신경쓰지 않음! (이미 LockItem에서 처리됨)
                    int removable = Mathf.Min(slot.count, remain);

                    Debug.Log($"[Remove] {removable}개 제거");

                    slot.count -= removable;

                    // locked도 함께 감소 (중요!)
                    if (slot.locked > 0)
                    {
                        int lockedToRemove = Mathf.Min(slot.locked, removable);
                        slot.locked -= lockedToRemove;
                        Debug.Log($"[Remove] locked도 {lockedToRemove}개 감소 → 남은 locked: {slot.locked}");
                    }

                    remain -= removable;

                    Debug.Log($"[Remove] 제거 후 - count: {slot.count}, locked: {slot.locked}");

                    // count가 0 이하가 되면 슬롯 비우기
                    if (slot.count <= 0)
                    {
                        Debug.Log($"[Remove] 슬롯 Clear 호출");
                        slot.Clear();
                    }

                    if (remain <= 0)
                        break;
                }
            }

            Debug.Log($"[Inventory.Remove] 완료 - 남은 제거량: {remain}");

            OnInventoryChanged?.Invoke();
        }

        #endregion

        #region Lock / Unlock

        public void LockItem(ItemData item, int count)
        {
            int remain = count;

            Debug.Log($"[Inventory.LockItem] {item.itemName} {count}개 잠금 시작");

            foreach (var slot in slots)
            {
                if (slot.item == item && remain > 0)
                {
                    int canLock = Mathf.Min(slot.AvailableCount, remain);
                    slot.locked += canLock;
                    remain -= canLock;

                    Debug.Log($"[LockItem] {canLock}개 잠금 - count: {slot.count}, locked: {slot.locked}, AvailableCount: {slot.AvailableCount}");

                    if (remain <= 0)
                        break;
                }
            }

            OnInventoryChanged?.Invoke();
        }

        public void UnlockItem(ItemData item, int count)
        {
            int remain = count;

            Debug.Log($"[Inventory.UnlockItem] {item.itemName} {count}개 잠금 해제 시작");

            foreach (var slot in slots)
            {
                if (slot.item == item && remain > 0)
                {
                    int unlock = Mathf.Min(slot.locked, remain);
                    slot.locked -= unlock;
                    remain -= unlock;

                    Debug.Log($"[UnlockItem] {unlock}개 해제 - count: {slot.count}, locked: {slot.locked}");

                    if (remain <= 0)
                        break;
                }
            }

            OnInventoryChanged?.Invoke();
        }

        #endregion

        #region Query

        public int GetItemCount(ItemData item)
        {
            int total = 0;

            foreach (var slot in slots)
            {
                if (slot.item == item)
                    total += slot.AvailableCount;
            }

            return total;
        }

        public List<InventorySlot> GetSlots()
        {
            return slots;
        }
        public bool HasItem(ItemData item, int requiredCount = 1)
        {
            return GetItemCount(item) >= requiredCount;
        }

        #endregion
    }
}