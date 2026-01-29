using Choi;
using TeamProject2;

public class BossEnemy : Enemy
{
    private BossPhase1State phase1 = new BossPhase1State();

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
}
