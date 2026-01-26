using UnityEngine;
using Choi;

namespace TeamProject2
{
    /// <summary>
    /// 체력의 초기값 설정 - 데이터 컨네이너
    /// </summary>
    [CreateAssetMenu(fileName = "HealthConfigSO", menuName = "EntityConfig/Health Config")]
    public class HealthConfigSO : ScriptableObject
    {
        [SerializeField] protected float _initialHealth = 50f;

        public float InitialHealth => _initialHealth;
    }
}