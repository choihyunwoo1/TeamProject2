using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public ShopItem testItem;   // 임시 하나만
    public ShopSlot slot;
    public GameObject shopPanel;
    public TMP_Text goldText;

    private void Start()
    {
        slot.SetItem(testItem);
        //시작할 때는 아직 PlayerGold 준비 안 됐을 수 있어서 호출 X
        // RefreshGold();
    }

    public void Open()
    {
        shopPanel.SetActive(true);
        RefreshGold();   //상점 열 때만 갱신
    }

    public void Close()
    {
        shopPanel.SetActive(false);
    }

    public void RefreshGold()
    {
        if (PlayerGold.Instance == null)
        {
            Debug.LogError("PlayerGold.Instance 없음");
            return;
        }

        if (goldText == null)
        {
            Debug.LogError("goldText 연결 안 됨");
            return;
        }

        goldText.text = PlayerGold.Instance.gold.ToString();
    }
}
