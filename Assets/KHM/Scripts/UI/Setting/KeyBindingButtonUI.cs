using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class KeyBindingButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI keyText;

    private bool isWaitingForKey = false;

    private void Update()
    {
        if (!isWaitingForKey) return;

        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            foreach (KeyControl key in Keyboard.current.allKeys)
            {
                if (key.wasPressedThisFrame)
                {
                    keyText.text = key.displayName;
                    isWaitingForKey = false;
                    break;
                }
            }
        }

        // 마우스도 받기
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            keyText.text = "Mouse Left";
            isWaitingForKey = false;
        }
        else if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            keyText.text = "Mouse Right";
            isWaitingForKey = false;
        }
    }

    public void StartRebinding()
    {
        keyText.text = "Press Any Key...";
        isWaitingForKey = true;
    }
}
