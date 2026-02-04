using System;
using UnityEngine;

namespace HJ
{
    public class SpawnManager : MonoBehaviour
    {
        //Inspector 창 안에서 캐릭터 프리팹 삽입해주세요
        //0 : 남캐, 1 : 여캐 입니다.
        public GameObject[] characterPrefabs;

        public Transform spawnPoint;

        private void Start()
        {
            int character = DataManager.selectedCharacter;
            Instantiate(characterPrefabs[character], spawnPoint.position, spawnPoint.rotation);
        }
    }
}