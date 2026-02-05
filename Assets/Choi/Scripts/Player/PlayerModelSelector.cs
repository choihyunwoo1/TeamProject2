using HJ;
using UnityEngine;

namespace Choi
{
    public class PlayerModelSelector : MonoBehaviour
    {
        public GameObject maleModel;
        public GameObject femaleModel;

        private void Awake()
        {
            maleModel.SetActive(false);
            femaleModel.SetActive(false);

            if (DataManager.selectedCharacter == 0)
                maleModel.SetActive(true);
            else
                femaleModel.SetActive(true);
        }
    }
}