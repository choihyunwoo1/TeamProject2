using System.Collections.Generic;
using UnityEngine;

namespace hm
{
    /// <summary>
    /// 
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        //툴팁
        [SerializeField] private TooltipController tooltipController;

        //팝업
        [Header("Popup")]
        [SerializeField] private List<PopupUIBase> popups;
        private PopupUIBase Get(PopupType type) => popupMap[type];

        private Dictionary<PopupType, PopupUIBase> popupMap;


        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            popupMap = new Dictionary<PopupType, PopupUIBase>();
            foreach (var popup in popups)
            {
                popupMap.Add(popup.Type, popup);
                popup.gameObject.SetActive(false);
            }
        }

        #region Tooltip
        //툴팁 보여주기
        public void ShowTooltip(ITooltipData data)
        {
            tooltipController.Show(data);
        }

        //툴팁 숨기기
        public void HideTooltip()
        {
            tooltipController.HideAll();
        }

        #endregion

        #region Popup
        public bool AnyPopupOpen()
        {
            foreach (var popup in popupMap.Values)
            {
                if (popup.IsOpen)
                    return true;
            }
            return false;
        }

        //I : 인벤토리
        //설정이 열려 있으면 무시
        public void HandleInventory()
        {
            var inventory = Get(PopupType.Inventory);
            var settings = Get(PopupType.Setting);

            if (settings.IsOpen)
                return;

            if (inventory.IsOpen)
                inventory.Hide();
            else
                inventory.Show();
        }

        //M : 지도
        //열 때 인벤이 열려 있으면 인벤 닫기, 설정 열려 있으면 무시
        public void HandleMap()
        {
            var map = Get(PopupType.Map);
            var inventory = Get(PopupType.Inventory);
            var settings = Get(PopupType.Setting);

            if (settings.IsOpen)
                return;

            if (map.IsOpen)
            {
                map.Hide();
            }
            else
            {
                inventory.Hide();
                map.Show();
            }
        }

        //ESC
        //팝업 하나라도 열려 있으면 하나 닫기, 아무 것도 없으면 설정 열기
        public void HandleEscape()
        {
            var settings = Get(PopupType.Setting);

            if (AnyPopupOpen())
            {
                CloseAllPopups();
            }
            else
            {
                settings.Show();
            }
        }
    
        public void TogglePopup(PopupType type)
        {
            if (!popupMap.TryGetValue(type, out var popup)) return;

            bool isOpen = popup.gameObject.activeSelf;
            CloseAllPopups();

            if (!isOpen)
                popup.Show();
        }
        public void CloseAllPopups()
        {
            foreach (var popup in popupMap.Values)
                popup.Hide();
        }

        //마우스 버튼 클릭으로 팝업창 열기
        public void OpenInventory()
        {
            HandleInventory();
        }

        public void OpenMap()
        {
            HandleMap();
        }

        public void OpenSetting()
        {
            HandleEscape();
        }
        #endregion
    }
}