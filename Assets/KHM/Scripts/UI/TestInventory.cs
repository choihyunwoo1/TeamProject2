using Choi;
using System.Collections.Generic;
using UnityEngine;

namespace hm
{
    public class TestInventory : MonoBehaviour, IInventory
    {
        [System.Serializable]
        public class TestItem
        {
            public ItemData item;
            public int count;
        }

        [SerializeField] private List<TestItem> items = new();

        private Dictionary<ItemData, int> lockedItems = new();

        public int GetItemCount(ItemData item)
        {
            var data = items.Find(x => x.item == item);
            return data != null ? data.count : 0;
        }

        public bool HasItem(ItemData item, int count)
        {
            return GetItemCount(item) >= count;
        }

        public void RemoveItem(ItemData item, int count)
        {
            var data = items.Find(x => x.item == item);
            if (data == null) return;

            data.count -= count;
            if (data.count < 0) data.count = 0;
        }

        public void LockItem(ItemData item, int count)
        {
            lockedItems[item] = count;
        }

        public void UnlockItem(ItemData item, int count)
        {
            if (!lockedItems.ContainsKey(item)) return;

            lockedItems[item] -= count;
            if (lockedItems[item] <= 0)
                lockedItems.Remove(item);
        }
    }
}
