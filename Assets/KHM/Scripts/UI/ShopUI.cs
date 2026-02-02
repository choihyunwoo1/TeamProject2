using UnityEngine;
using System.Collections.Generic;
using Choi;

namespace hm
{
    /// <summary>
    /// 상점 UI 메인
    /// 상점 아이템 슬롯들을 관리하고 표시
    /// </summary>
    public class ShopUI : MonoBehaviour
    {
        [Header("Slot Container")]
        [SerializeField] private Transform slotContainer;
        [SerializeField] private ShopSlotUI slotPrefab;

        private List<ShopSlotUI> shopSlots = new List<ShopSlotUI>();

        private void Start()
        {
            InitializeShop();
        }

        /// <summary>
        /// 상점 초기화 - ShopManager에서 아이템 목록 가져와서 슬롯 생성
        /// </summary>
        private void InitializeShop()
        {
            if (ShopManager.Instance == null)
            {
                Debug.LogError("[ShopUI] ShopManager가 없습니다!");
                return;
            }

            var shopItems = ShopManager.Instance.GetShopItems();

            // 기존 슬롯 제거
            foreach (var slot in shopSlots)
            {
                if (slot != null)
                    Destroy(slot.gameObject);
            }
            shopSlots.Clear();

            // 새 슬롯 생성
            foreach (var item in shopItems)
            {
                if (item != null)
                {
                    var slotObj = Instantiate(slotPrefab, slotContainer);
                    slotObj.Initialize(item);
                    shopSlots.Add(slotObj);
                }
            }
        }

        /// <summary>
        /// 상점 UI 새로고침
        /// </summary>
        public void RefreshShop()
        {
            foreach (var slot in shopSlots)
            {
                if (slot != null)
                {
                    slot.RefreshUI();
                }
            }
        }
    }
}