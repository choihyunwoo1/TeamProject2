using Choi;
using UnityEngine;
using UnityEngine.UI;

namespace hm
{
    public class WeaponButtonUI : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private WeaponItemData weapon;

        [Header("UI")]
        [SerializeField] private GameObject lockIcon;
        [SerializeField] private Button button;

        private WeaponUpgradeSystem upgradeSystem;

        /// <summary>
        /// 외부에서 시스템 주입
        /// </summary>
        public void Init(WeaponUpgradeSystem system)
        {
            upgradeSystem = system;
            Refresh();
        }

        /// <summary>
        /// 해금 여부에 따른 UI 갱신
        /// </summary>
        public void Refresh()
        {
            bool unlocked = upgradeSystem.IsWeaponUnlocked(weapon);

            lockIcon.SetActive(!unlocked);
            button.interactable = unlocked;
        }

        /// <summary>
        /// 무기 클릭 → 장착 + 스킬셋 변경
        /// </summary>
        public void OnClick()
        {
            if (!button.interactable) return;

            //스킬셋 변경
            UIManager.Instance.ChangeSkillSet(weapon.skillSet);

            //무기 장착
            
        }
    }
}
