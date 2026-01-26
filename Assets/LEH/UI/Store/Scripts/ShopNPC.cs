using UnityEngine;
using UnityEngine.InputSystem;

public class ShopNPC : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            ShopManager.Instance.OpenShop();
        }
    }
}
