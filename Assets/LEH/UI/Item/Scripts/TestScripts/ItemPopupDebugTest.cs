using UnityEngine;
using UnityEngine.InputSystem;

public class ItemPopupDebugTest : MonoBehaviour
{
    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.f8Key.wasPressedThisFrame)
        {
            ItemAcquirePopupManager.Instance.ShowMessage("테스트 아이템 x1 획득");
        }
    }
}
