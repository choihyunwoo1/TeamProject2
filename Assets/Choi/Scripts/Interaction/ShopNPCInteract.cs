using UnityEngine;
using hm;

namespace Choi
{
    public class ShopNPCInteract : MonoBehaviour, IInteractable
    {
         public string GetInteractPrompt()
         {
             return "Chat : [E]";
         }

         public void Interact(GameObject player)
         {
            UIManager.Instance.OpenShop();
         }
    } 
}