using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace hm
{
    public class InsertedMaterialSlotUI : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI countText;

        private ItemData currentItem;
        private int count;

        public void Set(ItemData item, int amount)
        {
            currentItem = item;
            count = amount;

            icon.sprite = item.icon;
            icon.enabled = true;

            countText.text = count > 1 ? count.ToString() : "";
            countText.gameObject.SetActive(true);
        }

        public void Clear()
        {
            currentItem = null;
            count = 0;

            icon.sprite = null;
            icon.enabled = false;

            countText.text = "";
            countText.gameObject.SetActive(false);
        }
    }
}
