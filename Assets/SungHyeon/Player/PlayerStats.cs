using UnityEngine;
using UnityEngine.Events;

namespace TeamProject2
{
    /// <summary>
    /// 플레이어 스텟을 관리하는 클래스
    /// </summary>
    public class PlayerStats : MonoBehaviour
    {
        #region Variables
        //플레이어 체력
        private float health;
        [SerializeField]
        private float maxHealth = 100f;

        //소지금
        private static int money;

        //초기 소지금
        [SerializeField]
        private int startMoney = 400;
        #endregion

        #region Property
        public float Health { get { return health; } }

        //소지금 읽기 전용 속성
        public static int Money { get { return money; }}
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //초기화
            money = startMoney; //초기 소지금 지급

        }
        #endregion

        #region Custom Method

        //돈 벌기
        public static void AddMoney(int amount)
        {
            money += amount;
        }

        //돈 쓰기
        public static bool UseMoney(int amount)
        {
            //소지금 체크
            if (money < amount)
            {
                Debug.Log("돈이 부족합니다");
                return false;
            }

            money -= amount;
            return true;
        }

        //소지금 체크
        public static bool HasMoney(int amount)
        {
            return money >= amount;
        }

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
        #endregion
    }
}