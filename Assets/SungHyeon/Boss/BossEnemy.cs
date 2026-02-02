using Choi;
using TeamProject2;
using UnityEngine;

public class BossEnemy : Enemy
{
    private BossPhase1State phase1 = new BossPhase1State();

    public bool IsInPhase1 = false;

    public GameObject fireZonePrefab;
    public Transform fireZoneSpawnPoint;
    public Transform FireBreathPivot;
    public GameObject fireBreathParticlePrefab;

    protected override void Start()
    {
        base.Start();
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
