using UnityEngine;
using UnityEngine.Events;
using Choi;

namespace TeamProject2
{
    /// <summary>
    /// 데미지를 관리하는 클래스
    /// Health 연산
    /// </summary>
    public class Damageable : MonoBehaviour, IDamageable
    {
        #region Variables
        [Header("Health")]
        [SerializeField] protected HealthConfigSO _healthConfigSO;
        [SerializeField] protected HealthSO _currentHealthSO;

        //무적 타이머
        [SerializeField] protected float invulnerabiltyTime = 0.5f;
        protected float m_timeSinceLastHit = 0f;

        //이벤트 함수
        public event UnityAction<float> OnDamage;
        public event UnityAction OnDie;
        #endregion

        #region Property
        public bool IsInvulnerable { get; private set; }    //무적 체크
        public bool IsDeath { get; private set; }           //죽음 체크
        public HealthSO CurrentHeathSO => _currentHealthSO;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            if (_healthConfigSO == null)
            {
                Debug.LogError("HealthConfigSO가 연결되지 않았습니다.");
                return;
            }

            if (_currentHealthSO == null)
            {
                _currentHealthSO = ScriptableObject.CreateInstance<HealthSO>();
                _currentHealthSO.SetMaxHealth(_healthConfigSO.InitialHealth);
                _currentHealthSO.SetCurrentHealth(_healthConfigSO.InitialHealth);
            }
            else
            {
                _currentHealthSO.SetCurrentHealth(_currentHealthSO.MaxHealth);
            }
        }

        private void Update()
        {
            if (IsDeath)
                return;

            if (IsInvulnerable)
            {
                m_timeSinceLastHit += Time.deltaTime;
                if (m_timeSinceLastHit >= invulnerabiltyTime)
                {
                    IsInvulnerable = false;
                    m_timeSinceLastHit = 0f;
                }
            }
        }
        #endregion

        #region Custom Method
        public void TakeDamage(float damage)
        {
            if (IsDeath)
                return;

            if (IsInvulnerable)
                return;

            _currentHealthSO.InflictDamage(damage);
            Debug.Log($"CurrentHealth: {_currentHealthSO.CurrentHealth}");

            OnDamage?.Invoke(damage);

            if (_currentHealthSO.CurrentHealth <= 0)
            {
                Die();
                return;
            }

            IsInvulnerable = true;
            m_timeSinceLastHit = 0f;
        }

        private void Die()
        {
            if (IsDeath)
                return;

            IsDeath = true;
            Debug.Log("Die");

            OnDie?.Invoke();

            //Destroy(gameObject); // 필요 시 활성화
        }

        public void Kill()
        {
            if (IsDeath)
                return;

            Die();
        }

        public void Revive()
        {
            _currentHealthSO.SetCurrentHealth(_currentHealthSO.MaxHealth);
            IsDeath = false;
            IsInvulnerable = false;
            m_timeSinceLastHit = 0f;
        }

        public void Cure(float healthAdd)
        {
            if (IsDeath)
                return;

            _currentHealthSO.RestoreHealth(healthAdd);
        }
        #endregion
    }
}
