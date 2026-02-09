using UnityEngine;

namespace Choi
{
    public class Lava : MonoBehaviour
    {
        [SerializeField] private bool instantKill = true; // true면 무조건 Die() 호출

        private void OnTriggerEnter(Collider other)
        {
            PlayerStats player = other.GetComponent<PlayerStats>();

            if (player != null)
            {
                if (instantKill)
                {
                    // 직접 죽이기
                    player.InstantKill();
                }
            }
        }
    }
}