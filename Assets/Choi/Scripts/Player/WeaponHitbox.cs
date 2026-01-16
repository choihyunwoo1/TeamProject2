using UnityEngine;

namespace Choi
{
    public class WeaponHitbox : MonoBehaviour
    {
        private Collider hitbox;

        void Awake()
        {
            hitbox = GetComponent<Collider>();
            hitbox.enabled = false;
        }

        public void EnableHitbox()
        {
            hitbox.enabled = true;
        }

        public void DisableHitbox()
        {
            hitbox.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IDamageable>(out var target))
            {
                target.TakeDamage(10f);
            }
        }
    }
}