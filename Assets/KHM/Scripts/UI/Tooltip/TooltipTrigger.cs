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
        [SerializeField] private bool useCustomPosition = false; // 커스텀 위치 사용 여부
        [SerializeField] private Vector2 offset = new Vector2(10f, 0f); // 오프셋 (기본: 오른쪽으로 10픽셀)

        private ITooltipData Data => tooltipData as ITooltipData;
        private RectTransform rectTransform;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void SetData(ITooltipData data)
        {
            tooltipData = data as ScriptableObject;
        }

        public void ClearData()
        {
            tooltipData = null;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (Data == null) return;

            if (useCustomPosition && rectTransform != null)
            {
                UIManager.Instance.ShowTooltip(Data, rectTransform, offset);
            }
            else
            {
                UIManager.Instance.ShowTooltip(Data);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            UIManager.Instance.HideTooltip();
        }

        /// <summary>
        /// 커스텀 위치 사용 설정
        /// </summary>
        public void SetUseCustomPosition(bool use, Vector2 customOffset = default)
        {
            useCustomPosition = use;
            if (customOffset != default)
                offset = customOffset;
        }
    }
}