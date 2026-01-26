using UnityEngine;

namespace Choi
{
    public class Breakable : MonoBehaviour, IDamageable
    {
        [Header("Health Settings")]
        [SerializeField] private float maxHealth = 10f;
        private float currentHealth;

        [Header("Fragments (Optional)")]
        [SerializeField] private Transform fragmentRoot;
        private Rigidbody[] fragments;

        [Header("Drop Item (Optional)")]
        [SerializeField] private GameObject dropItemPrefab;
        private bool isBroken = false;

        private void Awake()
        {
            currentHealth = maxHealth;

            if (fragmentRoot != null)
            {
                fragments = fragmentRoot.GetComponentsInChildren<Rigidbody>();

                foreach (var rb in fragments)
                {
                    rb.isKinematic = true;
                    rb.useGravity = false;
                }
            }
            else
            {
                fragments = new Rigidbody[0];
            }
        }

        public void TakeDamage(float damage)
        {
            if (isBroken) return;

            currentHealth -= damage;

            if (currentHealth <= 0f)
                Break();
        }

        private void Break()
        {
            isBroken = true;

            // 파편 물리 활성화
            foreach (var rb in fragments)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                rb.AddExplosionForce(200f, transform.position, 2f);
            }

            // 아이템 드롭
            SpawnDropItem();

            // 10초 뒤 파편 삭제
            if (fragmentRoot != null)
                Destroy(fragmentRoot.gameObject, 10f);

            Destroy(gameObject, 10f);
        }

        private void SpawnDropItem()
        {
            if (dropItemPrefab == null)
                return;

            Vector3 dropPos = fragmentRoot != null ? fragmentRoot.position : transform.position;

            Instantiate(dropItemPrefab, dropPos, Quaternion.identity);
        }
    }
}
