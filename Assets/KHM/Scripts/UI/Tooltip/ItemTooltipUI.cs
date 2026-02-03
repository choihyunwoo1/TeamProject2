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

            nameText.text = itemData.itemName;
            descriptionText.text = itemData.description;
            subtitleText.text = itemData.subtitle;

            // effectText와 conditionText 둘 다 체크
            bool hasEffect = !string.IsNullOrEmpty(itemData.effectText);
            bool hasCondition = !string.IsNullOrEmpty(itemData.conditionText);

            if (hasEffect && hasCondition)
            {
                // 둘 다 있으면 쉼표로 연결
                flavorText.text = $"{itemData.effectText}, {itemData.conditionText}";
            }
            else if (hasEffect)
            {
                // effectText만 있으면
                flavorText.text = itemData.effectText;
            }
            else if (hasCondition)
            {
                // conditionText만 있으면
                flavorText.text = itemData.conditionText;
            }
            else
            {
                // 둘 다 없으면 빈 문자열
                flavorText.text = "";
            }

            // ⭐️ 커스텀 위치가 아닐 때는 위치 리셋 (고정 위치로)
            ResetPosition();

            gameObject.SetActive(true);
        }
    }
}