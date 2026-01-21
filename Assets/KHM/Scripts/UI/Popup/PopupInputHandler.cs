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
                UIManager.Instance.HandleInventory();

            input.UI.Map.performed += _ =>
                UIManager.Instance.HandleMap();

            input.UI.Setting.performed += _ =>
                UIManager.Instance.HandleEscape();
        }

        private void OnDisable()
        {
            input.UI.Disable();
        }
    }
}