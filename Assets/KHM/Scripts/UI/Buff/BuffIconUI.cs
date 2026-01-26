using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace hm
{
    public class BuffIconUI : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TMP_Text stackText;
        [SerializeField] private TooltipTrigger tooltipTrigger;

        public void SetData(BuffData data)
        {
            iconImage.sprite = data.icon;
            tooltipTrigger.SetData(data);
        }

        public void SetStack(int count)
        {
            stackText.gameObject.SetActive(count > 1);
            stackText.text = count.ToString();
        }

    }
}