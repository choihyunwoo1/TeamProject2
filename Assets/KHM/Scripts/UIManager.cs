using UnityEngine;

namespace hm
{
    /// <summary>
    /// 
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [SerializeField] private TooltipController tooltipController;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        #region Tooltip

        public void ShowTooltip(ITooltipData data)
        {
            tooltipController.Show(data);
        }

        public void HideTooltip()
        {
            tooltipController.HideAll();
        }

        #endregion
    }
}