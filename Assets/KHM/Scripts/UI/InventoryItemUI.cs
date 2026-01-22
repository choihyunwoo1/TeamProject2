using UnityEngine;
using UnityEngine.UI;

namespace hm
{
    public class InventoryItemUI : MonoBehaviour
    {
        [SerializeField] private Image iconImage;

        [SerializeField] private ItemData itemData; //테스트용 직렬화
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
            UIManager.Instance.SelectItemForQuickSlot(itemData);
        }
    }
}
