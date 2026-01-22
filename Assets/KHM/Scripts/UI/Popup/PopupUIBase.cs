using UnityEngine;

namespace hm
{
    public abstract class PopupUIBase : MonoBehaviour
    {
        public abstract PopupType Type { get; }

        public bool IsOpen => gameObject.activeSelf;

        public virtual void Show()
        {
            gameObject.SetActive(true);
            Time.timeScale = 0f;
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
