using hm;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponPopupUI : MonoBehaviour
{
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private TextMeshProUGUI confirmButtonText;

    private WeaponItemData currentWeapon;
    private WeaponUpgradeSystem system;
    private RectTransform rectTransform;

    private void Awake()
    {
        system = WeaponUpgradeSystem.Instance;
        rectTransform = GetComponent<RectTransform>();
    }
   
    /// <summary>
    /// 무기 버튼 클릭 시 호출 - 제작 또는 장착 팝업
    /// </summary>
    /// <param name="weapon">무기 데이터</param>
    /// <param name="buttonTransform">클릭한 버튼의 RectTransform (위치 참조용)</param>
    public void Open(WeaponItemData weapon, RectTransform buttonTransform = null)
    {
        if (weapon == null)
        {
            Debug.LogError("무기 데이터가 null입니다.");
            return;
        }

        currentWeapon = weapon;

        // 버튼 리스너 초기화
        confirmButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();

        // 이미 해금되었는지 확인
        bool isUnlocked = system.IsUnlocked(weapon);

        if (isUnlocked)
        {
            // 이미 해금됨 - 장착 모드
            if (confirmButtonText != null)
                confirmButtonText.text = "무기장착";

            confirmButton.onClick.AddListener(OnClickEquip);
        }
        else
        {
            // 미해금 - 제작 모드
            if (confirmButtonText != null)
                confirmButtonText.text = "무기개조";

            confirmButton.onClick.AddListener(OnClickCraft);
        }

        cancelButton.onClick.AddListener(OnClickCancel);

        gameObject.SetActive(true);

        // 버튼 위치 기반으로 팝업 위치 설정
        if (buttonTransform != null)
        {
            SetPositionRelativeToButton(buttonTransform);
        }
    }

    /// <summary>
    /// 버튼 오른쪽에 팝업 위치 설정
    /// </summary>
    private void SetPositionRelativeToButton(RectTransform buttonTransform)
    {
        if (rectTransform == null) return;

        // 버튼의 월드 위치 가져오기
        Vector3 buttonWorldPos = buttonTransform.position;

        // 오른쪽으로 70 픽셀 이동
        Vector3 popupWorldPos = buttonWorldPos + new Vector3(70f, 0f, 0f);

        // 팝업 위치 설정
        rectTransform.position = popupWorldPos;
    }

    // 제작하기
    private void OnClickCraft()
    {
        if (system == null || currentWeapon == null) return;

        var recipe = system.GetRecipeByWeapon(currentWeapon);
        if (recipe == null)
        {
            Debug.LogError("레시피를 찾을 수 없습니다.");
            return;
        }

        system.Craft(recipe);
        Debug.Log($"{currentWeapon.itemName} 제작 완료!");

        gameObject.SetActive(false);
    }

    // 장착하기
    private void OnClickEquip()
    {
        if (currentWeapon == null) return;

        //장착 중인 무기 표시
        WeaponUpgradeSystem.Instance.EquipWeapon(currentWeapon);

        Debug.Log($"{currentWeapon.itemName} 장착!");

        gameObject.SetActive(false);
    }

    // 취소
    private void OnClickCancel()
    {
        gameObject.SetActive(false);
    }
}