using Choi;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace hm
{
    public class UpgradeMaterialSlotUI : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Image mask;
        [SerializeField] private TextMeshProUGUI countText;

        private ItemData item;

        public void Set(ItemData item, int filled, int required)
        {
            this.item = item;
            icon.sprite = item.icon;
            countText.text = $"{filled}/{required}";
            mask.enabled = filled < required;
        }

        public void Clear()
        {
            icon.sprite = null;
            countText.text = "";
            mask.enabled = true;
        }
    }
}