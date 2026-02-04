using UnityEngine;

public class SettingsTabManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject soundPanel;
    [SerializeField] private GameObject interfacePanel;
    [SerializeField] private GameObject keyBindingPanel;

    private GameObject currentPanel;

    private void Start()
    {
        // 시작할 때 GamePanel 보여주기
        ShowGame();
    }

    private void ShowPanel(GameObject panel)
    {
        if (currentPanel != null)
            currentPanel.SetActive(false);

        panel.SetActive(true);
        currentPanel = panel;
    }

    public void ShowGame()
    {
        ShowPanel(gamePanel);
    }

    public void ShowSound()
    {
        ShowPanel(soundPanel);
    }

    public void ShowInterface()
    {
        ShowPanel(interfacePanel);
    }

    public void ShowKeyBinding()
    {
        ShowPanel(keyBindingPanel);
    }
}
