using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace hm
{
    public class QuickSlotSlotUI : MonoBehaviour,
        IPointerEnterHandler, IPointerExitHandler
    {
        [Header("UI")]
        [SerializeField] private Image iconImage;
        [SerializeField] private GameObject glowSoft;
        [SerializeField] private GameObject glowHard;

        [Header("Index")]
        [SerializeField] private int slotIndex;

        private ItemData currentItem;
        private TooltipTrigger tooltipTrigger;
        private bool isSelectable;

        private void Awake()
        {
            tooltipTrigger = GetComponent<TooltipTrigger>();

            if (glowSoft) glowSoft.SetActive(false);
            if (glowHard) glowHard.SetActive(false);
            if (iconImage) iconImage.enabled = false;
        }

        public void SetSelectable(bool value)
        {
            isSelectable = value;
            glowSoft.SetActive(value);
            glowHard.SetActive(false);
        }
        public void SetItem(ItemData item)
        {
            currentItem = item;
            iconImage.sprite = item.icon;
            iconImage.enabled = true;

            tooltipTrigger.SetData(item);
        }

        public void Clear()
        {
            currentItem = null;
            iconImage.sprite = null;
            iconImage.enabled = false;
            tooltipTrigger.ClearData();
        }

        //슬롯 클릭 (Button OnClick으로 연결)
        public void OnClick()
        {
            if (!isSelectable)
                return;

            UIManager.Instance.AssignItemToSlot(slotIndex);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!isSelectable) return;
            glowHard.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            glowHard.SetActive(false);
        }
    }
}
