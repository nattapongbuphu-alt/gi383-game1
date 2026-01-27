using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance { get; private set; }

    public float elapsedTime { get; private set; }
    public float lastRunTime { get; private set; }
    public bool running = true;

    [Header("Countdown Settings")]
    public bool useCountdown = false;
    public float timeLimit = 60f; // seconds for countdown
    public float remainingTime { get; private set; }

    void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);

        elapsedTime = 0f;
        lastRunTime = 0f;
        running = true;
        remainingTime = timeLimit;
    }

    void Update()
    {
        if (!running) return;

        // normal elapsed time (total play time)
        elapsedTime += Time.deltaTime;

        if (useCountdown)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0f)
            {
                remainingTime = 0f;
                OnTimeUp();
            }
        }
    }

    public void StopTimer()
    {
        if (!running) return;
        running = false;

        // record last run time differently for countdown vs normal
        if (useCountdown)
        {
            lastRunTime = elapsedTime; // how long the player lasted
        }
        else
        {
            lastRunTime = elapsedTime;
        }
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        lastRunTime = 0f;
        running = true;
        remainingTime = timeLimit;
    }

    public float GetLastRunTime()
    {
        return lastRunTime;
    }

    public float GetRemainingTime()
    {
        return remainingTime;
    }

    void OnTimeUp()
    {
        // Stop and notify UI of game over
        running = false;
        lastRunTime = elapsedTime;

        var ui = FindObjectOfType<UI>();
        if (ui != null)
        {
            ui.ShowGameOver();
        }
        else
        {
            Time.timeScale = 0f;
        }
    }
}
