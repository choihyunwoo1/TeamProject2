using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Choi;

namespace hm
{
    public class UpgradeMaterialSlotUI : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Image mask; // 조건 미충족 시 마스크
        [SerializeField] private TextMeshProUGUI countText;

        private RequiredMaterial material;

        // 레시피 재료 초기화
        public void Init(RequiredMaterial material)
        {
            this.material = material;

            if (material != null && material.item != null)
            {
                icon.sprite = material.item.icon;
                icon.enabled = true;
            }

            Refresh(0); // 처음엔 0개로 시작
        }

        // 삽입된 재료 수량에 따라 UI 갱신
        public void Refresh(int insertedCount)
        {
            if (material == null) return;

            // 수량 텍스트 업데이트
            countText.text = $"{insertedCount}/{material.count}";

            // 마스크 처리: 필요한 만큼 채워지면 마스크 해제
            bool isSatisfied = insertedCount >= material.count;
            mask.enabled = !isSatisfied;
        }

        // 슬롯 비우기
        public void Clear()
        {
            material = null;
            icon.sprite = null;
            icon.enabled = false;
            countText.text = "";
            mask.enabled = true;
        }

        // 현재 슬롯의 아이템 반환
        public ItemData GetItem()
        {
            return material?.item;
        }

        // 필요한 수량 반환
        public int GetRequiredCount()
        {
            return material?.count ?? 0;
        }
    }
}