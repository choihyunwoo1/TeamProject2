using UnityEngine;
namespace hm
{
    public abstract class TooltipUIBase : MonoBehaviour
    {
        protected RectTransform rectTransform;
        protected CanvasGroup canvasGroup;
        protected Vector2 initialPosition; // ⭐️ 초기 위치 저장

        protected virtual void Awake()
        {
            rectTransform = GetComponent<RectTransform>();

            // ⭐️ 초기 위치 저장 (Start가 아닌 Awake에서)
            initialPosition = rectTransform.anchoredPosition;

            // CanvasGroup이 없으면 추가 (레이캐스트 차단용)
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
            // ⭐️ 툴팁은 레이캐스트를 받지 않도록 설정 (깜빡임 방지)
            canvasGroup.blocksRaycasts = false;
        }

        public abstract void Show(ITooltipData data);

        /// <summary>
        /// 특정 UI 요소 옆에 툴팁 표시
        /// </summary>
        public virtual void Show(ITooltipData data, RectTransform targetRect, Vector2 offset)
        {
            Show(data);
            PositionTooltip(targetRect, offset);
        }

        /// <summary>
        /// 툴팁 위치를 초기 위치로 리셋 (고정 위치용)
        /// </summary>
        protected virtual void ResetPosition()
        {
            if (rectTransform == null) return;

            // ⭐️ 저장된 초기 위치로 리셋 (예: 330, -810)
            rectTransform.anchoredPosition = initialPosition;
        }

        /// <summary>
        /// 툴팁 위치 조정
        /// </summary>
        protected virtual void PositionTooltip(RectTransform targetRect, Vector2 offset)
        {
            if (rectTransform == null || targetRect == null) return;

            // 타겟의 월드 포지션 가져오기
            Vector3[] targetCorners = new Vector3[4];
            targetRect.GetWorldCorners(targetCorners);

            // 타겟의 오른쪽 중간 지점 계산
            Vector3 rightMiddle = (targetCorners[2] + targetCorners[3]) / 2f;

            // 툴팁 위치 설정 (왼쪽 중간을 기준으로)
            Vector3[] tooltipCorners = new Vector3[4];
            rectTransform.GetWorldCorners(tooltipCorners);
            Vector3 tooltipLeftMiddle = (tooltipCorners[0] + tooltipCorners[1]) / 2f;

            Vector3 offsetPosition = rightMiddle - tooltipLeftMiddle;
            rectTransform.position += offsetPosition + new Vector3(offset.x, offset.y, 0);

        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}