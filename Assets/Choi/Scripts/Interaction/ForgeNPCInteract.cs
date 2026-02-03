using UnityEngine;
using hm;

namespace Choi
{
    public class ForgeNPCInteract : MonoBehaviour, IInteractable
    {
        public string GetInteractPrompt()
        {
            return "Chat : [E]";
        }

        public void Interact(GameObject player)
        {
            UIManager.Instance.OpenWeaponUpgrade();
        }
    }
}