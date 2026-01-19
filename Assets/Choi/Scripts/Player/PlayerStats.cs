using UnityEngine;
using System.Collections;

namespace Choi
{
    public class PlayerStats : MonoBehaviour, IDamageable
    {
        [Header("HP")]
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float invincibleDuration = 0.3f;

        [Header("Gauge")]
        public float maxGauge = 100f;
        public float currentGauge = 0f;

        [Header("DamageBlink")]
        [SerializeField] private float blinkInterval = 0.1f;   // 깜빡임 간격
        [SerializeField] private float transparentAlpha = 0.4f; // 반투명 정도

        public float CurrentHealth { get; private set; }
        public bool IsDead { get; private set; }
        public bool IsInvincible { get; private set; }

        private Animator animator;
        private Renderer[] renderers;
        private Color[] originalColors;

        private void Awake()
        {
            CurrentHealth = maxHealth;
            animator = GetComponent<Animator>();

            // 자식 오브젝트까지 포함한 모든 Renderer 가져오기
            renderers = GetComponentsInChildren<Renderer>();

            // 원래 색 저장
            originalColors = new Color[renderers.Length];
            for (int i = 0; i < renderers.Length; i++)
            {
                originalColors[i] = renderers[i].material.color;
            }
        }

        public void TakeDamage(float damage)
        {
            if (IsDead || IsInvincible) return;

            CurrentHealth -= damage;
            animator.SetTrigger("Hit");

            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                Die();
                return;
            }

            StartCoroutine(InvincibilityRoutine());
        }

        private IEnumerator InvincibilityRoutine()
        {
            IsInvincible = true;

            float timer = 0f;

            while (timer < invincibleDuration)
            {
                // 1. 반투명 처리
                SetTransparency(transparentAlpha);

                // 2. 짧게 깜빡임
                ToggleRenderers(false);
                yield return new WaitForSeconds(blinkInterval * 0.5f);
                ToggleRenderers(true);
                yield return new WaitForSeconds(blinkInterval * 0.5f);

                timer += blinkInterval;
            }

            // 원래 상태 복구
            ResetTransparency();
            ToggleRenderers(true);

            IsInvincible = false;
        }

        private void SetTransparency(float alpha)
        {
            foreach (var rend in renderers)
            {
                Color c = rend.material.color;
                c.a = alpha;
                rend.material.color = c;

                // Rendering Mode가 Opaque인 경우 반투명 적용 안되므로 처리
                if (rend.material.HasProperty("_Surface"))
                    rend.material.SetFloat("_Surface", 1); // Transparent 모드
            }
        }

        private void ResetTransparency()
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.color = originalColors[i];

                if (renderers[i].material.HasProperty("_Surface"))
                    renderers[i].material.SetFloat("_Surface", 0); // Opaque로 복구
            }
        }

        private void ToggleRenderers(bool state)
        {
            foreach (var rend in renderers)
            {
                rend.enabled = state;
            }
        }

        private void Die()
        {
            IsDead = true;
            animator.SetTrigger("Death");
        }

        public void AddGauge(float amount)
        {
            Debug.Log("게이지 증가 호출됨 " + amount);
            currentGauge = Mathf.Clamp(currentGauge + amount, 0, maxGauge);
        }

        public bool ConsumeGauge(float amount)
        {
            if (currentGauge < amount)
                return false;

            currentGauge -= amount;
            return true;
        }
    }
}