using TMPro;
using UnityEngine;

namespace hm
{
    public class BuffTooltipUI : TooltipUIBase
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private TMP_Text subtitleText;


        public override void Show(ITooltipData data)
        {
            var buffData = (BuffData)data;

            nameText.text = buffData.buffName;
            descriptionText.text = buffData.description;
            subtitleText.text = buffData.subtitle;

            gameObject.SetActive(true);
        }
    }
}