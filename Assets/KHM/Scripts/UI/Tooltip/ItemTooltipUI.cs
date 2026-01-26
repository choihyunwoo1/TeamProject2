using TMPro;
using UnityEngine;
namespace Choi
{
    public class ItemTooltipUI : TooltipUIBase
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descriptionText;

        public override void Show(ITooltipData data)
        {
            var itemData = (ItemData)data;

            nameText.text = itemData.itemName;
            descriptionText.text = itemData.description;

            gameObject.SetActive(true);
        }
    }
}