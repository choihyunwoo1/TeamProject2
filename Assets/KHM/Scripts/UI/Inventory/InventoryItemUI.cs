using UnityEngine;
using UnityEngine.UI;

namespace hm
{
    public class InventoryItemUI : MonoBehaviour
    {
        [SerializeField] private Image iconImage;

        [SerializeField] private ItemData itemData; //테스트용 직렬화
        [SerializeField] private WeaponUpgradeSystem upgradeSystem;
        [SerializeField] private WeaponUpgradeUI upgradeUI;
        [SerializeField] private UpgradeSelectPopupUI popupUI;

        private TooltipTrigger tooltipTrigger;

        private void Awake()
        {
            tooltipTrigger = GetComponent<TooltipTrigger>();
        }

        public void SetItem(ItemData data)
        {
            itemData = data;
            iconImage.sprite = data.icon;

            tooltipTrigger.SetData(itemData);
        }

        public void Clear()
        {
            itemData = null;
            iconImage.sprite = null;
            tooltipTrigger.ClearData();
        }
       
        public void OnClick()
        {
            if (itemData == null) return;

            if (itemData.itemType != ItemType.Material)
                return;

            UIManager.Instance.SelectItemForQuickSlot(itemData);

            popupUI.Open(
                itemData,
                upgradeSystem,
                upgradeUI,
                GetComponent<RectTransform>()  
            );
        }

        public void OnClickUpgrade()
        {
            if (itemData.itemType != ItemType.Material)
                return;

            int filled = upgradeSystem.TryFillMaterial(itemData);

            if (filled > 0)
                upgradeUI.Refresh();
        }

    }
}
