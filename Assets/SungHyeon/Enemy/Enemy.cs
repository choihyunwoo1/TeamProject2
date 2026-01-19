using UnityEngine;
using UnityEngine.AI;

namespace TeamProject2
{
    public abstract class Enemy : MonoBehaviour, IDamageable
    {
        #region Variables
        protected Animator animator;     //애니메이터
        protected NavMeshAgent agent;    //이동
        protected Transform target;      //플레이어

        protected EnemyState currentState;   //현재 상태
        protected EnemyState beforeState;    //이전 상태
        protected bool isDeath;               //사망 여부

        [SerializeField] protected float maxHealth; //최대 체력
        protected float currentHealth;              //현재 체력

        [SerializeField] protected int rewardGold;  //골드
        [SerializeField] protected int rewardExp;   //경험치
        [SerializeField] protected GameObject dropItem; //아이템

        [SerializeField] protected float destroyDelay; //삭제 지연
       
        protected const string MOVE_SPEED = "MoveSpeed"; //이동 속도
        protected const string IS_DEATH = "IsDeath";     //사망
        #endregion

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>(); //애니메이터 참조
            agent = GetComponent<NavMeshAgent>(); //에이전트 참조
        }

        protected virtual void Start()
        {
            currentHealth = maxHealth; //체력 초기화
        }

        protected virtual void Update()
        {
            if (isDeath) return; //사망 체크

            if (agent != null)
            {
                animator.SetFloat(MOVE_SPEED, agent.velocity.magnitude); //이동 애니메이션
            }
        }

        public virtual void SetState(EnemyState newState)
        {
            if (currentState == newState) return; //상태 중복 방지

            beforeState = currentState; //이전 상태 저장
            currentState = newState;    //상태 변경

            agent.ResetPath(); //이동 초기화

            if (currentState == EnemyState.E_Death)
            {
                animator.SetBool(IS_DEATH, true); //사망 애니메이션
                agent.enabled = false;            //이동 중지
            }
        }

        public virtual void TakeDamage(float damage)
        {
            if (isDeath) return; //사망 체크

            currentHealth -= damage; //체력 감소

            if (currentHealth <= 0f)
            {
                Die(); //사망
            }
        }

        protected virtual void Die()
        {
            isDeath = true; //사망 처리

            SetState(EnemyState.E_Death); //상태 변경

            GiveReward(); //보상 지급
            DropItem();   //아이템 드랍

            Destroy(gameObject, destroyDelay); //삭제
        }

        protected virtual void GiveReward()
        {
            PlayerStats.AddMoney(rewardGold); //골드 지급
            PlayerStats.AddExp(rewardExp);    //경험치 지급
        }

        protected virtual void DropItem()
        {
            if (dropItem != null)
            {
                Instantiate(
                    dropItem,
                    transform.position + Vector3.up * 0.5f, //드랍 위치
                    Quaternion.identity
                );
            }
        }

        public virtual void SetTarget(Transform target)
        {
            this.target = target; //타겟 설정
        }
    }
}
