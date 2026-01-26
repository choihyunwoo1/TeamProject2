namespace hm
{
    public interface IInventory
    {
        int GetItemCount(ItemData item);

        bool HasItem(ItemData item, int count);

        void RemoveItem(ItemData item, int count);

        void LockItem(ItemData item, int count);

        void UnlockItem(ItemData item, int count);
    }
}
