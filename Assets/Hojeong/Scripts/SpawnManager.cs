using UnityEngine;
using Choi;

namespace HJ
{
    public class SpawnManager : MonoBehaviour
    {
        // 0 = 남캐 프리팹, 1 = 여캐 프리팹
        public GameObject[] characterPrefabs;

        // 스폰 위치
        public Transform spawnPoint;

        private void Start()
        {
            int selected = DataManager.selectedCharacter;

            // 안전 체크
            if (characterPrefabs == null || characterPrefabs.Length == 0)
            {
                Debug.LogError("SpawnManager: 캐릭터 프리팹이 설정되지 않았습니다.");
                return;
            }

            if (selected < 0 || selected >= characterPrefabs.Length)
            {
                Debug.LogError($"SpawnManager: 잘못된 캐릭터 값({selected}) 입니다.");
                return;
            }

            // 이미 생성된 Player가 있는가?
            var existingPlayer = FindFirstObjectByType<PlayerStats>();
            if (existingPlayer != null)
            {
                Debug.Log("SpawnManager: 이미 플레이어가 존재하여 생성하지 않습니다.");
                gameObject.SetActive(false); // 스스로 비활성화
                return;
            }

            Debug.Log("SpawnManager: 캐릭터 생성됨");

            // Player 생성
            GameObject playerObj = Instantiate(
                characterPrefabs[selected],
                spawnPoint.position,
                spawnPoint.rotation
            );

            // 생성된 캐릭터 스케일 강제 조정
            playerObj.transform.localScale = new Vector3(1.9f, 1.9f, 1.9f);

            // 한 번 사용했으니 off
            gameObject.SetActive(false);

        }
    }
}
