using UnityEngine;
using UnityEngine.Events;
using Choi;

namespace TeamProject2
{
    /// <summary>
    /// 플레이어 스텟을 관리하는 클래스
    /// </summary>
    public class PlayerStats : MonoBehaviour
    {
        [SerializeField] private float attackDamage;

        #region Variables
        //플레이어 체력
        private float health;
        [SerializeField]
        private float maxHealth = 100f;


        #endregion

        #region Property
        public float Health { get { return health; } }


        #endregion

        #region Unity Event Method
        private void Start()
        { 

        }
        #endregion

        #region Custom Method
        public void PlayerStatsInitialize(PlayData playData)
        {
            if (playData != null)
            {
                health = playData.health;
            }
            else
            { 
                health = maxHealth;
            }
        }

        //체력 저장
        public void SetHealth(float value)
        { 
            health = value;
        }

        private void Attack()
        {
            IDamageable damageable = GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(attackDamage);
            }
        }
        #endregion
    }
}