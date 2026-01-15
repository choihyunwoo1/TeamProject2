using UnityEngine;

namespace TeamProject2
{
    namespace TeamProject2
    {
        public class Elite : Enemy
        {
            #region Detect
            [SerializeField] private float detectDistance; //감지 거리
            [SerializeField] private float attackRange;    //공격 거리
            #endregion

            #region Move
            [SerializeField] private float moveSpeed; //이동 속도
            #endregion

            #region NormalAttack
            [SerializeField] private float attackDamage;   //기본 공격력
            [SerializeField] private float attackCooldown; //기본 공격 쿨타임
            private float attackTimer;                      //공격 타이머
            #endregion

            #region EliteSkill
            [SerializeField] private float skillDamage;    //강력한 공격 데미지
            [SerializeField] private float skillCooldown;  //강력한 공격 쿨타임
            private float skillTimer;                       //스킬 타이머
            #endregion

            #region Animator
            private const string ATTACK = "Attack"; //기본 공격
            private const string SKILL = "Skill";   //강력한 공격
            #endregion

            private void Update()
            {
                if (isDeath) return; //사망 체크
                if (target == null) return; //타겟 체크

                float distance = Vector3.Distance(target.position, transform.position); //거리 계산

                if (distance <= attackRange)
                {
                    SetState(EnemyState.E_Attack); //공격
                }
                else if (distance <= detectDistance)
                {
                    SetState(EnemyState.E_Chase); //추격
                }
                else
                {
                    SetState(EnemyState.E_Idle); //대기
                }

                HandleState(); //상태 처리
            }

            private void HandleState()
            {
                switch (currentState)
                {
                    case EnemyState.E_Idle:
                        Idle(); //대기
                        break;

                    case EnemyState.E_Chase:
                        Chase(); //추격
                        break;

                    case EnemyState.E_Attack:
                        Attack(); //공격
                        break;
                }
            }

            private void Idle()
            {
                agent.SetDestination(transform.position); //정지
            }

            private void Chase()
            {
                agent.speed = moveSpeed; //속도 설정
                agent.SetDestination(target.position); //플레이어 추적
            }

            private void Attack()
            {
                agent.SetDestination(transform.position); //정지

                attackTimer += Time.deltaTime; //기본 공격 타이머
                skillTimer += Time.deltaTime;  //스킬 타이머

                if (skillTimer >= skillCooldown)
                {
                    Skill(); //강력한 공격
                    skillTimer = 0f;
                    attackTimer = 0f;
                    return;
                }

                if (attackTimer >= attackCooldown)
                {
                    NomalAttack(); //기본 공격
                    attackTimer = 0f;
                }
            }

            private void NomalAttack()
            {
                animator.SetTrigger(ATTACK); //기본 공격 애니메이션

                IDamageable damageable = target.GetComponent<IDamageable>(); //데미지 인터페이스
                if (damageable != null)
                {
                    currentHealth -= attackDamage;

                    damageable.TakeDamage(attackDamage); //데미지
                }
            }

            private void Skill()
            {
                animator.SetTrigger(SKILL); //스킬 애니메이션

                IDamageable damageable = target.GetComponent<IDamageable>(); //데미지 인터페이스
                if (damageable != null)
                {
                    damageable.TakeDamage(skillDamage); //강력한 데미지
                }
            }

            private void OnDrawGizmosSelected()
            {
                Gizmos.color = Color.yellow; //감지
                Gizmos.DrawWireSphere(transform.position, detectDistance); //감지 범위

                Gizmos.color = Color.red; //공격
                Gizmos.DrawWireSphere(transform.position, attackRange); //공격 범위
            }
        }
    }

}

