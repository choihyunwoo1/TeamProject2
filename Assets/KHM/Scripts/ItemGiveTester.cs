using UnityEngine;

namespace hm
{
    public class ItemGiveTester : MonoBehaviour
    {
        [SerializeField] private ItemDatabase itemDB;
        [SerializeField] private WeaponUpgradeUI upgradeUI;

        [ContextMenu("Give Test Set")]
        public void GiveTestSet()
        {
            upgradeUI.OpenUpgradeUI();
            Inventory.Instance.Add(itemDB.GetByName("antidote"), 2);
            Inventory.Instance.Add(itemDB.GetByName("matScarp"), 10);
            Inventory.Instance.Add(itemDB.GetByName("matSharp"), 10);
            Inventory.Instance.Add(itemDB.GetByName("matStone"), 10);
            Inventory.Instance.Add(itemDB.GetByName("matString"), 30);
            Inventory.Instance.Add(itemDB.GetByName("matWood"), 30);
            Debug.Log("여러 아이템 지급 완료");
        }
    }
}
