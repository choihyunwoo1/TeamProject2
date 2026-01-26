using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace hm
{
    /// <summary>
    /// 스킬 쿨타임을 보여주는 UI를 관리하는 클래스
    /// 외부에서 StartCooldown(duration)을 호출받아 동작한다
    /// </summary>
    public class SkillCooldownUI : MonoBehaviour
    {
        [SerializeField] private Image cooldownMask;
        //[SerializeField] private TMP_Text cooldownText;

        private float cooldown;
        private float remaining;

        public void StartCooldown(float duration)
        {
            cooldown = duration;
            remaining = duration;

            cooldownMask.gameObject.SetActive(true);
            UpdateUI();
        }

        private void Update()
        {
            if (remaining <= 0f ) return;

            remaining -= Time.unscaledDeltaTime;
            UpdateUI();

            if (remaining <= 0f)
                EndCooldown();
        }

        private void UpdateUI()
        {
            cooldownMask.fillAmount = remaining / cooldown;
            //cooldownText.text = Mathf.Ceil(remaining).ToString();
        }

        private void EndCooldown()
        {
            cooldownMask.gameObject.SetActive(false);
            //cooldownText.text = "";
        }
    }
}