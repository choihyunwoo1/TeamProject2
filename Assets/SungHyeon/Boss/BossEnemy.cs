using Choi;
using hm;
using UnityEngine;

namespace TeamProject2
{
    public class BossEnemy : Enemy
    {
        private BossPhase1State phase1 = new BossPhase1State();

        public bool IsInPhase1 = false;

        public GameObject fireZonePrefab;
        public Transform fireZoneSpawnPoint;
        public Transform FireBreathPivot;
        public GameObject fireBreathParticlePrefab;

        [SerializeField] private float extraHealth = 50f;
        protected override void Start()
        {
            base.Start();

            // 체력 보정 로직은 Start에서!
            Damageable dmg = GetComponent<Damageable>();
            if (dmg != null)
            {
                dmg.CurrentHeathSO.SetMaxHealth(dmg.MaxHealth + extraHealth);
                dmg.CurrentHeathSO.SetCurrentHealth(dmg.CurrentHeathSO.MaxHealth);

                Debug.Log($"Boss HP boosted: {dmg.CurrentHeathSO.MaxHealth}");
            }

            // Phase1 등록
            stateMachine.RegisterState(phase1);

            damageable.OnDamage += CheckPhase;
        }

        private void CheckPhase(float currentHP)
        {
            float hpRate = currentHP / damageable.MaxHealth;

            if (hpRate <= 0.5f && !IsInPhase1)
            {
                IsInPhase1 = true;
                stateMachine.ChangeState(phase1);

                damageable.OnDamage -= CheckPhase;
            }
        }
        public void SpawnFireZone()
        {
            Instantiate(fireZonePrefab, fireZoneSpawnPoint.position, Quaternion.identity);
        }

        public void SpawnFireBreath()
        {
            if (fireBreathParticlePrefab != null && FireBreathPivot != null)
            {
                GameObject effect = Instantiate(
                    fireBreathParticlePrefab,
                    FireBreathPivot.position,
                    FireBreathPivot.rotation);

                effect.transform.SetParent(FireBreathPivot);
            }
        }
    }
}