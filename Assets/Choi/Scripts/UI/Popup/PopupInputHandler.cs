using UnityEngine;

namespace hm
{
    public class PopupInputHandler : MonoBehaviour
    {
        private InputSystem_Actions input;

        private void Awake()
        {
            input = new InputSystem_Actions();
        }

        private void OnEnable()
        {
            input.UI.Enable();

            input.UI.Inventory.performed += _ =>
            {
                if (IsPopupInputBlocked()) return;
                UIManager.Instance.HandleInventory();
            };

            input.UI.Map.performed += _ =>
            {
                if (IsPopupInputBlocked()) return;
                UIManager.Instance.HandleMap();
            };

            input.UI.Setting.performed += _ =>
            {
                if (IsPopupInputBlocked()) return;
                UIManager.Instance.HandleEscape();
            };
        }

        private bool IsPopupInputBlocked()
        {
            if (InventoryUI.Instance == null) return false;

            var mode = InventoryUI.Instance.GetCurrentMode();

            return mode == InventoryMode.WeaponUpgrade
                || mode == InventoryMode.Shop;
        }

        private void OnDisable()
        {
            input.UI.Disable();
        }
    }
}