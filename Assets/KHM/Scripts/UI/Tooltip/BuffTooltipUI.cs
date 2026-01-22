using TMPro;
using UnityEngine;

namespace hm
{
    public class BuffTooltipUI : TooltipUIBase
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descriptionText;

        public override void Show(ITooltipData data)
        {
            var buffData = (BuffData)data;

            nameText.text = buffData.buffName;
            descriptionText.text = buffData.description;

            gameObject.SetActive(true);
        }
    }
}