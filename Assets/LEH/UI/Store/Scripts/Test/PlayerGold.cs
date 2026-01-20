using UnityEngine;

public class PlayerGold : MonoBehaviour
{
    public static PlayerGold Instance;
    public int gold = 1000;

    private void Awake()
    {
        Instance = this;
    }
}
