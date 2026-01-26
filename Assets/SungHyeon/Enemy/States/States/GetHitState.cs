using TeamProject2;
using UnityEngine;

public class GetHitState : State
{
    private Animator animator;
    private float timer;
    private const float hitDuration = 0.3f; // 히트 애니메이션 길이

    public override void OnInitalize()
    {
        animator = enemy.GetComponent<Animator>();
    }

    public override void OnEnter()
    {
        timer = 0f;
        animator.SetTrigger("GetHit");
    }

    public override void OnUpdate(float deltaTime)
    {
        timer += deltaTime;

        // 히트 애니메이션 끝나면 Idle로 복귀
        if (timer >= hitDuration)
            enemy.ChangeState(new IdleState());
    }

    public override void OnExit() { }
}
