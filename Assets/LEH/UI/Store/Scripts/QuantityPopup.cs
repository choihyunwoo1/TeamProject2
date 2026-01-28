using UnityEngine;
using TMPro;
using System;

public class QuantityPopup : MonoBehaviour
{
    public TMP_Text quantityText;
    int currentAmount;
    Action<int> onConfirm;

    public void Open(int maxAmount, Action<int> callback)
    {
        currentAmount = 1;
        onConfirm = callback;
        UpdateText();
        gameObject.SetActive(true);
    }

    public void Increase()
    {
        currentAmount++;
        UpdateText();
    }

    public void Decrease()
    {
        if (currentAmount > 1)
            currentAmount--;
        UpdateText();
    }

    public void Confirm()
    {
        onConfirm?.Invoke(currentAmount);
        gameObject.SetActive(false);
    }

    void UpdateText()
    {
        quantityText.text = currentAmount.ToString();
    }
}
