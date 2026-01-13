using UnityEngine;

namespace TeamProject2
{
    /// <summary>
    /// 데미지를 관리하는 클래스
    /// Health를 관리하는 클래스
    /// </summary>
    public interface Damageable 
    {
        public void TakeDamage(float damage);
    }
}