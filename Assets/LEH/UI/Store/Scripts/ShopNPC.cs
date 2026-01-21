using UnityEngine;
using UnityEngine.InputSystem;

public class ShopNPC : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (Keyboard.current == null) return;

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Debug.Log("상점 열기");

            // 여기서 상점 열기 함수 호출
            // 예:
            // FindFirstObjectByType<ShopManager>().Open();
        }
    }
}
