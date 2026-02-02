using TMPro;
using UnityEngine;
namespace hm
{
    public class ItemTooltipUI : TooltipUIBase
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private TMP_Text flavorText;
        [SerializeField] private TMP_Text subtitleText;


        public override void Show(ITooltipData data)
        {
            var itemData = (ItemData)data;
            string _flavorText = $"{itemData.effectText}, {itemData.conditionText}";

            nameText.text = itemData.itemName;
            descriptionText.text = itemData.description;
            flavorText.text = _flavorText;
            subtitleText.text = itemData.subtitle;

            gameObject.SetActive(true);
        }
    }
}