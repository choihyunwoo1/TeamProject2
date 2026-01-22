using UnityEngine;

public class PlayerGold : MonoBehaviour
{
    public static PlayerGold Instance;

    public int gold = 1000; // 시작 골드

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 바뀌어도 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddGold(int amount)
    {
        gold += amount;
    }

    public bool UseGold(int amount)
    {
        if (gold < amount)
            return false;

        gold -= amount;
        return true;
    }
}
