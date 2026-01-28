using UnityEngine;
using UnityEngine.AI;

namespace TeamProject2
{
    //적 공통 상태 정의
    public enum EnemyState
    {
        E_Idle,         //대기
        E_Walk,         //패트롤
        E_GetHit,          
        E_Attack,       //공격
        E_Die         //죽음
    }


    /// <summary>
    /// Enemy를 관리하는 클래스
    /// </summary>
    public class EnemyStats : IDamageable
    {
        #region Variables
        //참조
        private Animator animator;
        private NavMeshAgent agent;
        private Transform thePlayer;

        //상태 관리
        [SerializeField]
        private EnemyState currentState;    //현재 상태
        private EnemyState beforeState;     //이전 상태

        //
        [SerializeField] private Transform ThePlayer;
        

        //체력
        private float health;
        [SerializeField]
        private float maxHealth = 20f;
        //죽음
        private bool isDeath = false;
        [SerializeField]
        private float destoryDelay = 6f;

        //상태 - 대기
        private float idleTimer = 2f; //2~3초
        private float countdown = 0f;

        //상태 - 패트롤
        [SerializeField]
        private bool isPatrol = false;

        public Transform[] wayPoints;
        [SerializeField]
        private int wayPointIndex = 0;

        //처음 생성 위치
        private Vector3 startPosion = Vector3.zero;

        //상태 - 추격
        [SerializeField]
        private float detectDistance = 10f;     //적이 디텍트 거리안에 들어오면 추격 시작

        //상태 - 공격
        [SerializeField]
        private float attackRange = 5f;         //적이 사거리 안에 들어오면 추격을 멈추고 공격 시작
        [SerializeField]
        private float attackTimer = 2f;         //2초에 한번씩 발사
        [SerializeField]
        private float attackDamage = 5f;        //발사시 플레이어에게 attackDamage(5) 준다

        private bool isBack = false;

        //애니메이터 파라미터
        const string MoveSpeed = "ForwardSpeed";
        new const string IsDeath = "Die";
        const string Attack = "Attack";
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            animator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();

        }

        private void Start()
        {
            //초기화
            health = maxHealth;
            wayPointIndex = 0;
            startPosion = transform.position;

        }

        private void Update()
        {
      

            //애니메이터 파라미터 처리
            animator.SetFloat(MoveSpeed, agent.velocity.magnitude);
        }

        //디텍팅 거리 기즈모 그리기
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, detectDistance);

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(this.transform.position, attackRange);
        }
        #endregion

        #region Custom Method
       


        //데미지 처리
        public void TakeDamage(float damage)
        {
            health -= damage;

            //효과(vfx, sfx), UI, 애니메이션 
            animator.SetTrigger("Attack");

            if (health <= 0f && isDeath == false)
            {
                Die();
            }
        }

        //죽음 처리
        private void Die()
        {
            isDeath = true;

            animator.SetTrigger("Die");

            //킬
            Destroy(gameObject, destoryDelay);
        }
        #endregion
    }
}