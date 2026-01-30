using UnityEngine;
using TMPro;

namespace hm
{
    public class GoldUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI goldText;

        private void Start()
        {
            Inventory.Instance.OnGoldChanged += Refresh;
            Refresh(Inventory.Instance.Gold);
        }

        private void OnDestroy()
        {
            if (Inventory.Instance != null)
                Inventory.Instance.OnGoldChanged -= Refresh;
        }

        private void Refresh(int gold)
        {
            goldText.text = gold.ToString() + "G";
        }
    }
}
