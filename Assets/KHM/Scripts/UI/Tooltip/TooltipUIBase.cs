using UnityEngine;
namespace hm
{
    public abstract class TooltipUIBase : MonoBehaviour
    {
        public abstract void Show(ITooltipData data);

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}