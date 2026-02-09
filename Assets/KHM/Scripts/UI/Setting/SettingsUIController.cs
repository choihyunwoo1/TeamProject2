using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI 설정 값을 조절하는 클래스
/// </summary>
public class SettingsUIController : MonoBehaviour
{
   /* [Header("UI Scale")]
    [SerializeField] private Slider uiScaleSlider;
    [SerializeField] private CanvasScaler canvasScaler;*/

    [Header("Minimap Zoom")]
    [SerializeField] private Slider minimapZoomSlider;
    [SerializeField] private Camera minimapCamera;

    [Header("Toggles")]
    [SerializeField] private Toggle hudToggle;
    [SerializeField] private GameObject hudRoot;       

    private void Start()
    {
        /*uiScaleSlider.onValueChanged.AddListener(SetUIScale);*/
        minimapZoomSlider.onValueChanged.AddListener(SetMinimapZoom);
        hudToggle.onValueChanged.AddListener(SetHUDVisible);

        minimapZoomSlider.value = 10f;

        // 시작 시 값 반영
        /*SetUIScale(uiScaleSlider.value);*/
        SetMinimapZoom(minimapZoomSlider.value);
        SetHUDVisible(hudToggle.isOn);
    }

    #region UI Scale

    /*public void SetUIScale(float value)
    {
        canvasScaler.scaleFactor = value;
    }*/
    #endregion

    #region Minimap Zoom

    private void SetMinimapZoom(float value)
    {
        minimapCamera.orthographicSize = value;
    }

    #endregion

    #region Toggle Controls

    private void SetHUDVisible(bool isOn)
    {
        hudRoot.SetActive(isOn);
    }
    #endregion
}
