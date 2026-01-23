using UnityEngine;

public class PlayerGold : MonoBehaviour
{
    public static PlayerGold Instance;

    public int gold = 1000;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // ✔ 골드 충분한지
    public bool CanSpend(int amount)
    {
        return gold >= amount;
    }

    // ✔ 골드 차감
    public void SpendGold(int amount)
    {
        gold -= amount;
        if (gold < 0) gold = 0;
    }

    // ✔ 골드 추가
    public void AddGold(int amount)
    {
        gold += amount;
    }
}
