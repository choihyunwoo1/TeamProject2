using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace hm
{
    public class UpgradeSelectPopupUI : MonoBehaviour
    {
        [SerializeField] private Button actionButton;
        [SerializeField] private Button cancelButton;
        [SerializeField] private TextMeshProUGUI actionText;

        private ItemData currentItem;
        private WeaponUpgradeSystem upgradeSystem;
        private WeaponUpgradeUI upgradeUI;

        public void Open(ItemData item,
                         WeaponUpgradeSystem system,
                         WeaponUpgradeUI ui,
                         RectTransform slotRect)
        {
            currentItem = item;
            upgradeSystem = system;
            upgradeUI = ui;

            PositionNearSlot(slotRect);

            gameObject.SetActive(true);

            actionButton.onClick.RemoveAllListeners();

            if (upgradeSystem.IsMaterialFilled(item))
            {
                actionText.text = "재료 빼기";
                actionButton.onClick.AddListener(RemoveMaterial);
            }
            else
            {
                actionText.text = "재료 넣기";
                actionButton.onClick.AddListener(AddMaterial);
            }

            cancelButton.onClick.RemoveAllListeners();
            cancelButton.onClick.AddListener(Close);
        }

        private void PositionNearSlot(RectTransform slot)
        {
            Vector3 pos = slot.position;

            // 오른쪽에 팝업 띄우기
            pos.x += 160f;

            transform.position = pos;
        }

        private void AddMaterial()
        {
            int filled = upgradeSystem.TryFillMaterial(currentItem);
            if (filled > 0)
                upgradeUI.Refresh();

            Close();
        }

        private void RemoveMaterial()
        {
            int removed = upgradeSystem.TryRemoveMaterial(currentItem);
            if (removed > 0)
                upgradeUI.Refresh();

            Close();
        }

        private void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
