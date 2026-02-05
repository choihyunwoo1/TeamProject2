using UnityEngine;
using Choi;

namespace HJ
{
    public class SpawnManager : MonoBehaviour
    {
        public GameObject maleModel;
        public GameObject femaleModel;

        public Transform spawnPoint;

        private void Start()
        {
            int selected = DataManager.selectedCharacter;

            maleModel.SetActive(false);
            femaleModel.SetActive(false);

            if (selected == 0)
                maleModel.SetActive(true);
            else
                femaleModel.SetActive(true);

            // Player를 spawnPoint로 이동 (필요할 경우)
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;
        }
    }
}
