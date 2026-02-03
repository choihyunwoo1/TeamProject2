using hm;
using System.Collections.Generic;
using UnityEngine;

namespace Choi
{
    public class ChestInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private List<DropItem> dropItems = new();
        [SerializeField] private int dropGold = 0;

        private bool isOpened = false;

        public string GetInteractPrompt()
        {
            return isOpened ? "" : "Open (E)";
        }

        public void Interact(GameObject player)
        {
            if (isOpened) return;
            isOpened = true;

            GiveDropItems();
            GiveDropGold();

            Debug.Log("Chest Open!");

            // ⭐ 드랍 끝난 후 상자 제거
            Destroy(gameObject);
        }

        private void GiveDropItems()
        {
            if (Inventory.Instance == null)
            {
                Debug.LogError("Inventory.Instance is NULL!");
                return;
            }

            foreach (var drop in dropItems)
            {
                if (drop.item == null) continue;

                Inventory.Instance.Add(drop.item, drop.count);
                Debug.Log($"[Chest] {name} → {drop.item.itemName} {drop.count}개 지급 완료");
            }
        }

        private void GiveDropGold()
        {
            if (dropGold > 0 && Inventory.Instance != null)
            {
                Inventory.Instance.AddGold(dropGold);
                Debug.Log($"[Chest] {name} → Gold {dropGold} 지급 완료");
            }
        }
    }
}
