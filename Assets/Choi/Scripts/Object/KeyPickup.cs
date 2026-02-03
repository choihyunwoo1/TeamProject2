using hm;
using UnityEngine;

namespace Choi
{
    public class KeyPickup : MonoBehaviour, IInteractable
    {
        [SerializeField] private ItemData keyItem;
        [SerializeField] private int amount = 1;

        private bool picked = false;

        public string GetInteractPrompt()
        {
            return picked ? "" : "Pick Up Key (E)";
        }

        public void Interact(GameObject player)
        {
            if (picked) return;

            picked = true;
            Inventory.Instance.Add(keyItem, amount);

            Debug.Log($"[KeyPickup] {keyItem.itemName} 획득");

            Destroy(gameObject); // 주운 뒤 사라짐
        }
    }
}
