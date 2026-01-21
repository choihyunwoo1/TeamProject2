using UnityEngine;

namespace Choi
{
    public class PlayerSpawner : MonoBehaviour
    {
        public PortalSpawnPoint[] spawnPoints;

        private void Start()
        {
            string id = PortalManager.LastPortalID;
            if (string.IsNullOrEmpty(id))
                return;

            var player = FindObjectOfType<PlayerStats>().transform;

            foreach (var p in spawnPoints)
            {
                if (p.portalID == id)
                {
                    player.position = p.transform.position;
                    player.rotation = p.transform.rotation;
                    break;
                }
            }
        }
    }
}