using TMPro;
using UnityEngine;

public class ResolutionSelectorUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resolutionText;

    private string[] resolutions =
    {
        "1280 x 720",
        "1600 x 900",
        "1920 x 1080",
        "2560 x 1440",
        "3840 x 2160"
    };

    private int currentIndex = 2; // 기본 1920x1080

    private void Start()
    {
        UpdateText();
    }

    public void OnClickLeft()
    {
        currentIndex--;

        if (currentIndex < 0)
            currentIndex = resolutions.Length - 1;

        UpdateText();
    }

    public void OnClickRight()
    {
        currentIndex++;

        if (currentIndex >= resolutions.Length)
            currentIndex = 0;

        UpdateText();
    }

    private void UpdateText()
    {
        resolutionText.text = resolutions[currentIndex];
    }
}
