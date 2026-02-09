using hm;
using UnityEngine;

namespace Choi
{
    public class DoorInteractable : MonoBehaviour, IInteractable
    {
        [Header("Required Key")]
        public ItemData requiredKey;   // 필요한 열쇠 아이템

        public Animator animator;

        private bool isOpened = false;

        public string GetInteractPrompt()
        {
            // 이미 열린 경우 상호작용 문구 없음
            if (isOpened) return "";

            return requiredKey == null
                ? "Open : [E]"
                : $"Use {requiredKey.itemName} : [E]";
        }

        public void Interact(GameObject player)
        {
            if (isOpened) return;

            // 열쇠가 필요한 경우 → 체크
            if (requiredKey != null)
            {
                // 인벤토리에 키 없으면 실패
                if (!Inventory.Instance.HasItem(requiredKey))
                {
                    Debug.Log($"문을 열 수 없습니다. 필요한 열쇠: {requiredKey.itemName}");
                    return;
                }

                // 키 제거
                Inventory.Instance.Remove(requiredKey, 1);
                Debug.Log($"{requiredKey.itemName} 1개 사용됨");
            }

            // 문 열기
            animator.SetTrigger("Open");
            isOpened = true;

            Debug.Log("문이 열렸습니다.");
        }
    }
}
