using UnityEngine;
using UnityEngine.UI;

namespace hm
{
    public class WeaponButtonUI : MonoBehaviour
    {
        [SerializeField] private Image lockMask; // 제작 불가 시 마스크
        [SerializeField] private Image iconImage; // 무기 아이콘
        [SerializeField] private Button button;
        [SerializeField] private WeaponItemData weaponData;
        [SerializeField] private GameObject equippedRoot; // 무기 장착시 Glow + E

        [Header("Default Weapon")]
        [SerializeField] private bool isDefaultWeapon = false; // 기본 무기 여부

        public WeaponItemData Weapon => weaponData;
        private RectTransform rectTransform;

        private void Awake()
        {
            // 무기 아이콘 설정
            if (iconImage != null && weaponData != null && weaponData.icon != null)
            {
                iconImage.sprite = weaponData.icon;
                iconImage.enabled = true;
            }

            // RectTransform 가져오기
            rectTransform = GetComponent<RectTransform>();
        }

        // 제작 가능 여부에 따라 마스크 처리
        public void SetCraftable(bool canCraft)
        {
            // 기본 무기는 항상 마스크 없음
            if (isDefaultWeapon)
            {
                if (lockMask != null)
                    lockMask.gameObject.SetActive(false);
                if (button != null)
                    button.interactable = true;
                return;
            }

            // 일반 무기는 제작 가능 여부에 따라
            if (lockMask != null)
                lockMask.gameObject.SetActive(!canCraft);
            if (button != null)
                button.interactable = canCraft;
        }

        //무기 장착 시 글로우 효과
        public void RefreshEquipped(WeaponItemData equipped)
        {
            bool isEquipped = equipped == weaponData;

            if (equippedRoot != null)
                equippedRoot.SetActive(isEquipped);
        }

        // 무기 버튼 클릭 - Button 컴포넌트의 OnClick 이벤트에 연결
        public void OnClick()
        {
            Debug.Log($"무기 버튼 클릭: {weaponData?.itemName}");

            var system = WeaponUpgradeSystem.Instance;

            if (system == null)
            {
                Debug.LogError("WeaponUpgradeSystem.Instance가 null입니다.");
                return;
            }

            if (weaponData == null)
            {
                Debug.LogError("weaponData가 할당되지 않았습니다.");
                return;
            }

            // 기본 무기는 항상 팝업 열기
            if (isDefaultWeapon)
            {
                Debug.Log("기본 무기 - 팝업 열기");
                var upgradeUI = WeaponUpgradeUI.Instance; 
                upgradeUI?.OpenWeaponPopup(weaponData, GetComponent<RectTransform>());
                return;
            }

            // 일반 무기: 이미 해금되었거나 제작 가능한 경우에만 팝업 열기
            bool isUnlocked = system.IsUnlocked(weaponData);
            bool canCraft = false;

            if (!isUnlocked)
            {
                var recipe = system.GetRecipeByWeapon(weaponData);
                canCraft = recipe != null && system.CanCraftRecipe(recipe);
            }

            if (isUnlocked || canCraft)
            {
                var upgradeUI = WeaponUpgradeUI.Instance; 
                upgradeUI?.OpenWeaponPopup(weaponData, GetComponent<RectTransform>());
            }
            else
            {
                Debug.Log("재료가 부족합니다.");
            }
        }
    }
}