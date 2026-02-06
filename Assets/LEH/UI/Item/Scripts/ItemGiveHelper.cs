using UnityEngine;

namespace hm
{
    public static class ItemGiveHelper
    {
        public static void Give(ItemData item, int amount)
        {
            Inventory.Instance.Add(item, amount);

            if (ItemAcquirePopupManager.Instance != null)
            {
                ItemAcquirePopupManager.Instance
                    .ShowMessage($"{amount} {item.itemName} 획득");
            }
        }
    }
}
