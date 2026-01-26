using UnityEngine;

namespace Choi
{
    public class ChestInteractable : MonoBehaviour, IInteractable
    {
        [Header("Chest Settings")]
        [SerializeField] private GameObject[] dropItemPrefabs;   // 여러 개 넣을 수 있음
        [SerializeField] private Transform dropPoint;            // 아이템 생성 위치 (옵션)

        private bool isOpened = false;

        // 인터랙트 UI 문구
        public string GetInteractPrompt()
        {
            return isOpened ? "" : "Open (E)";
        }

        // 실제 상호작용 실행
        public void Interact(GameObject player)
        {
            if (isOpened) return;

            isOpened = true;

            // 필요하면 애니메이션 or 사운드 호출
            // animator.SetTrigger("Open");

            SpawnItems();

            Debug.Log("Chest Open!");
        }

        private void SpawnItems()
        {
            // 아무 아이템 없으면 그냥 return
            if (dropItemPrefabs == null || dropItemPrefabs.Length == 0)
                return;

            // 드롭 생성 위치 (없으면 Chest 중심)
            Vector3 spawnPos = dropPoint != null ? dropPoint.position : transform.position;

            foreach (var prefab in dropItemPrefabs)
            {
                if (prefab == null) continue; // 안전 처리

                Instantiate(prefab, spawnPos, Quaternion.identity);
            }
        }
    }
}
