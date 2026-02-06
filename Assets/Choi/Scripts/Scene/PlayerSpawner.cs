using HJ;
using UnityEngine;

namespace Choi
{
    public class PlayerSpawner : MonoBehaviour
    {
        public PortalSpawnPoint[] spawnPoints;

        // 0 = 남캐 프리팹, 1 = 여캐 프리팹
        public GameObject[] characterPrefabs;

        private void Start()
        {
            string id = PortalManager.LastPortalID;
            if (string.IsNullOrEmpty(id))
            {
                Debug.Log("PlayerSpawner: 마지막 포탈 ID가 없습니다.");
                return;
            }

            // 캐릭터 선택값 확인
            int selected = DataManager.selectedCharacter;

            if (characterPrefabs == null || characterPrefabs.Length == 0)
            {
                Debug.LogError("PlayerSpawner: 캐릭터 프리팹 배열이 비어있습니다.");
                return;
            }

            if (selected < 0 || selected >= characterPrefabs.Length)
            {
                Debug.LogError($"PlayerSpawner: 잘못된 캐릭터 선택값({selected}) 입니다.");
                return;
            }

            // 스폰 포인트 찾기
            PortalSpawnPoint target = null;
            foreach (var p in spawnPoints)
            {
                if (p.portalID == id)
                {
                    target = p;
                    break;
                }
            }

            if (target == null)
            {
                Debug.LogError($"PlayerSpawner: '{id}' 에 해당하는 스폰포인트를 찾지 못함.");
                return;
            }

            // 캐릭터 생성
            Instantiate(
                characterPrefabs[selected],
                target.transform.position,
                target.transform.rotation
            );
        }
    }
}
