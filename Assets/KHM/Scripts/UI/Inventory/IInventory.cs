namespace hm
{
    public interface IInventory
    {
        int GetItemCount(ItemData item);
        void Add(ItemData item, int count);
        void Remove(ItemData item, int count);
        void LockItem(ItemData item, int count);
        void UnlockItem(ItemData item, int count);
    }
}
