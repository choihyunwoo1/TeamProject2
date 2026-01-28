using UnityEngine;
using TMPro;

public class PlayerGold : MonoBehaviour
{
    public int currentGold = 1000;
    public TMP_Text goldText;

    void Start() => UpdateUI();

    public bool CanAfford(int amount) => currentGold >= amount;

    public void AddGold(int amount)
    {
        currentGold += amount;
        UpdateUI();
    }

    public bool SpendGold(int amount)
    {
        if (!CanAfford(amount)) return false;
        currentGold -= amount;
        UpdateUI();
        return true;
    }

    void UpdateUI()
    {
        if (goldText != null)
            goldText.text = currentGold.ToString();
    }
}
