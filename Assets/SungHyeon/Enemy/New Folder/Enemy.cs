using UnityEngine;
using Choi;

namespace TeamProject2
{
    /// <summary>
    /// 적을 관리하는 베이스 클래스, 모든 적들의 부모 클래스
    /// </summary>
    public class Enemy : MonoBehaviour
    {
        #region Variables
        //참조
        public DetectionModule m_DetectionMoudle;
        protected Damageable damageable;

        //상태 머신
        protected StateMachine stateMachine;

        private IdleState idleState = new IdleState();
        private WalkState walkState = new WalkState();
        private AttackState attackState = new AttackState();
        private DieState dieState = new DieState();
        private GetHitState getHitState = new GetHitState();

        //공격 범위
        [SerializeField] protected float attackRange = 2.0f;
        //공격 딜레이 타임
        [SerializeField] protected float attackDelayTime = 1f;

        //회전 속도 - Lerp 계수
        [SerializeField] protected float rotateSpeed = 10f;
        #endregion

        #region Property
        public IdleState IdleState => idleState;
        public WalkState WalkState => walkState;
        public AttackState AttackState => attackState;
        public DieState DieState => dieState;
        public GetHitState GetHitState => getHitState;

        public Transform Target => m_DetectionMoudle.Target;
        public DetectionModule Detection => m_DetectionMoudle;
        public float DetectionRange => m_DetectionMoudle.DetectionRange;
        public float AttackRange => attackRange;
        public float AttackDelayTime => attackDelayTime;
        //공격 가능 여부 체크
        public bool IsAttackable
        {
            get
            {
                if (Target)
                {
                    return (m_DetectionMoudle.DistanceToTarget <= attackRange);
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion

        #region Unity Event Method
        protected virtual void Awake()
        {
            //참조
            m_DetectionMoudle = GetComponent<DetectionModule>();
            damageable = GetComponent<Damageable>();
        }

        protected virtual void OnEnable()
        {
            damageable.OnDamage += OnDamaged;
            damageable.OnDie += OnDie;
            damageable.OnAttack += OnAttack;
        }

        protected virtual void OnDisable()
        {
            damageable.OnDamage -= OnDamaged;
            damageable.OnDie -= OnDie;
            damageable.OnAttack -= OnAttack;
        }

        protected virtual void Start()
        {
            stateMachine = new StateMachine(this, idleState);
            stateMachine.RegisterState(walkState);
            stateMachine.RegisterState(attackState);
            stateMachine.RegisterState(dieState);
            stateMachine.RegisterState(getHitState);
        }

        protected virtual void Update()
        {
            //상태머신의 업데이트 : 현재상태의 업데이트를 매 프레임마다 실행
            stateMachine.Update(Time.deltaTime);
        }
        #endregion

        #region Custom Method
        //상태 변경
        public State ChangeState(State newState)
        {
            return stateMachine.ChangeState(newState);
        }

        //타겟을 바라본다
        public void FaceToTarget()
        {
            //타겟 체크
            if (Target == null)
                return;

            //타겟의 방향을 구해 방향에 대한 Rotation을 얻는다
            Vector3 dir = (Target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(dir.x, transform.position.y, dir.z));
            transform.rotation = Quaternion.Slerp(transform.rotation,
                lookRotation, Time.deltaTime * rotateSpeed);
        }

        private void OnDamaged(float damage)
        {
            ChangeState(getHitState);
        }

        private void OnDie()
        {
            ChangeState(dieState);

            //킬
            Destroy(gameObject, 3f);
        }

        private void OnAttack(float damage)
        {
            ChangeState(attackState);
        }

        public void ClearTarget()
        {
            m_DetectionMoudle.ClearTarget();   // Target = null
        }
        #endregion
    }
}