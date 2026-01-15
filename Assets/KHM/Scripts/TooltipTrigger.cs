using UnityEngine;
using UnityEngine.EventSystems;

namespace hm
{
    /// <summary>
    /// 특정 UI에 마우스를 올렸을 때 툴팁을 띄워주는 트리거
    /// BuffIconUI, ItemSlotUI, SkillButton 등에 붙여서 사용
    /// </summary>
    public class TooltipTrigger : MonoBehaviour,
        IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private ScriptableObject tooltipData;

        private ITooltipData Data => tooltipData as ITooltipData;

        public void SetData(ScriptableObject data)
        {
            tooltipData = data;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (Data == null) return;
            UIManager.Instance.ShowTooltip(Data);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            UIManager.Instance.HideTooltip();
        }
    }
}