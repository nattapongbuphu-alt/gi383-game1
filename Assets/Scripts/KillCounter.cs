using UnityEngine;

public class KillCounter : MonoBehaviour
{
    public static KillCounter Instance { get; private set; }

    private int kills = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }

    public void AddKill(int amount = 1)
    {
        kills += amount;
    }

    public int GetKills()
    {
        return kills;
    }

    public void ResetKills()
    {
        kills = 0;
    }
}
