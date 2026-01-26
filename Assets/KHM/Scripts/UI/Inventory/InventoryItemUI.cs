using UnityEngine;
using UnityEngine.UI;

namespace Choi
{
    public class InventoryItemUI : MonoBehaviour
    {
        [SerializeField] private Image iconImage;

        [SerializeField] private ItemData itemData; //테스트용 직렬화
<<<<<<< HEAD:Assets/KHM/Scripts/UI/InventoryItemUI.cs
=======
        [SerializeField] private WeaponUpgradeSystem upgradeSystem;
        [SerializeField] private WeaponUpgradeUI upgradeUI;
        [SerializeField] private UpgradeSelectPopupUI popupUI;

>>>>>>> 4b6d073e3b18e6d046059e10fd840aa4764e3ebf:Assets/KHM/Scripts/UI/Inventory/InventoryItemUI.cs
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
    }
}
