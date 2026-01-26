using UnityEngine;
using UnityEngine.AI;
using Choi;

namespace TeamProject2
{
    public class EnemyStats : Damageable
    {
        #region Variables
        // Components
        private Animator animator;
        private NavMeshAgent agent;

        [SerializeField] private Transform ThePlayer;

        // Patrol
        [SerializeField] private bool isPatrol = false;
        public Transform[] wayPoints;
        private int wayPointIndex = 0;
        private Vector3 startPosition;

        // Timers
        private float idleTimer = 2f;
        private float countdown = 0f;

        // Chase / Attack
        [SerializeField] private float detectDistance = 10f;
        [SerializeField] private float attackRange = 5f;
        [SerializeField] private float attackTimer = 2f;
        [SerializeField] private float attackDamage = 5f;

        // Death
        [SerializeField] private float destroyDelay = 6f;

        // Animator parameters
        private const string MoveSpeed = "MoveSpeed";
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            animator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();

            // Damageable 이벤트 등록
            OnDamage += HandleHitReaction;
            OnDie += HandleDeath;
        }

        private void Start()
        {
            wayPointIndex = 0;
            startPosition = transform.position;
        }

        private void Update()
        {
            if (IsDeath)
                return;

            // Move animation
            animator.SetFloat(MoveSpeed, agent.velocity.magnitude);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, detectDistance);

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(this.transform.position, attackRange);
        }
        #endregion

        #region Damageable Event Callbacks
        // Damageable → 체력 감소 시 호출됨
        private void HandleHitReaction(float damage)
        {
            Debug.Log("Enemy Hittttttttttttttttttt");

        }

        // Damageable → 죽을 때 호출됨
        private void HandleDeath()
        {
            Debug.Log("Enemy DIeeeeeeeeeeeeeeeee");


            Destroy(gameObject, destroyDelay);
        }
        #endregion
    }
}
