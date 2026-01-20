using UnityEngine;

public class ShopNPC : MonoBehaviour
{
    public ShopManager shop;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") &&
            Input.GetKeyDown(KeyCode.E))
        {
            shop.Open();
        }
    }
}
