using UnityEngine;

namespace hm
{
    public class ItemGiveTester : MonoBehaviour
    {
        [SerializeField] private ItemDatabase itemDB;

        [ContextMenu("Give Test Set")]
        public void GiveTestSet()
        {
            UIManager.Instance.OpenWeaponUpgrade();
            Inventory.Instance.Add(itemDB.GetByName("antidote"), 2);
            Inventory.Instance.Add(itemDB.GetByName("matScarp"), 10);
            Inventory.Instance.Add(itemDB.GetByName("matSharp"), 10);
            Inventory.Instance.Add(itemDB.GetByName("matStone"), 10);
            Inventory.Instance.Add(itemDB.GetByName("matString"), 30);
            Inventory.Instance.Add(itemDB.GetByName("matWood"), 30);
            Debug.Log("여러 아이템 지급 완료");
        }

        [ContextMenu("Shop Test Set")]
        public void shopmode()
        {
            UIManager.Instance.OpenShop();
            Inventory.Instance.AddGold(1000);

        }
    }
}
