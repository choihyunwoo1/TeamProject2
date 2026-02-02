using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace hm
{
    public class UpgradeSelectPopupUI : MonoBehaviour
    {
        [SerializeField] private Button actionButton;
        [SerializeField] private Button cancelButton;
        [SerializeField] private TextMeshProUGUI actionText;

        private ItemData currentItem;
        private WeaponUpgradeSystem upgradeSystem;
        private WeaponUpgradeUI upgradeUI;
        private RectTransform rectTransform;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 팝업 열기
        /// </summary>
        /// <param name="item">아이템 데이터</param>
        /// <param name="system">무기 업그레이드 시스템</param>
        /// <param name="ui">무기 업그레이드 UI</param>
        /// <param name="slotTransform">클릭한 슬롯의 RectTransform (위치 참조용)</param>
        public void Open(ItemData item, WeaponUpgradeSystem system, WeaponUpgradeUI ui, RectTransform slotTransform = null)
        {
            if (item == null || system == null || ui == null)
            {
                Debug.LogError("팝업 열기 실패: null 파라미터");
                return;
            }

            currentItem = item;
            upgradeSystem = system;
            upgradeUI = ui;

            gameObject.SetActive(true);

            // 버튼 리스너 초기화
            actionButton.onClick.RemoveAllListeners();
            cancelButton.onClick.RemoveAllListeners();

            // 이미 삽입되었는지 확인
            int insertedCount = upgradeSystem.GetInsertedCount(item);
            bool isInserted = insertedCount > 0;

            if (isInserted)
            {
                // 재료 빼기
                actionText.text = "재료 빼기";
                actionButton.onClick.AddListener(RemoveMaterial);
            }
            else
            {
                // 재료 넣기
                actionText.text = "재료 넣기";
                actionButton.onClick.AddListener(AddMaterial);
            }

            actionButton.interactable = true;
            cancelButton.onClick.AddListener(Close);

            // 슬롯 위치 기반으로 팝업 위치 설정
            if (slotTransform != null)
            {
                SetPositionRelativeToSlot(slotTransform);
            }
        }

        /// <summary>
        /// 슬롯 오른쪽에 팝업 위치 설정
        /// </summary>
        private void SetPositionRelativeToSlot(RectTransform slotTransform)
        {
            if (rectTransform == null || slotTransform == null) return;

            // 슬롯의 월드 위치 가져오기
            Vector3 slotWorldPos = slotTransform.position;
            // 오른쪽으로 60 픽셀 이동
            Vector3 popupWorldPos = slotWorldPos + new Vector3(60f, 0f, 0f);
            // 팝업 위치 설정
            rectTransform.position = popupWorldPos;
        }

        private void AddMaterial()
        {
            if (currentItem == null || upgradeSystem == null) return;

            upgradeSystem.InsertMaterial(currentItem);
            Close();
        }

        private void RemoveMaterial()
        {
            if (currentItem == null || upgradeSystem == null) return;

            upgradeSystem.RemoveInsertedMaterial(currentItem);
            Close();
        }

        private void Close()
        {
            currentItem = null;
            upgradeSystem = null;
            upgradeUI = null;
            gameObject.SetActive(false);
        }
    }
}