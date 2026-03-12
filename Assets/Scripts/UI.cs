using Unity.Services.Core;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    public GameObject winPanel;
    public string playerTag = "Player";
    public bool pauseOnWin = true;
    private static int gameOver = 0;

    // retry count is maintained for the entire session and should not reset
    // when the scene reloads, so make it static and keep the UI object alive.
    public static int retry = 1;
    public bool isGameOver = false;
    public static UI instance;

    void Awake()
    {
        // simple singleton reference for convenience; instances are recreated with each scene
        instance = this;
    }

    [Header("Win Panel Time Display")]
    public Text uiTimeText; // optional legacy UI.Text
    public TextMeshProUGUI tmpTimeText; // optional TextMeshPro
    [Header("Win Panel Kill Display")]
    public Text uiKillsText;
    public TextMeshProUGUI tmpKillsText;
    [Header("Game Over Panel")]
    public GameObject gameOverPanel;
    public bool pauseOnGameOver = true;

    void Start()
    {
        if (winPanel != null)
            winPanel.SetActive(false);

        Initialize();
    }

    private async void Initialize() 
    {
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
    }

    public void ShowWin()
    {
        // Ensure final time is recorded if timer exists
        var timer = FindObjectOfType<GameTimer>();
        if (timer != null)
            timer.StopTimer();

        // Update time and kill labels before pausing
        UpdateWinTimeLabel();
        UpdateWinKillLabel();

        if (winPanel != null)
            winPanel.SetActive(true);

        if (pauseOnWin)
            Time.timeScale = 0f;
    }

    public void HideWin()
    {
        if (winPanel != null)
            winPanel.SetActive(false);

        if (pauseOnWin)
            Time.timeScale = 1f;
    }

    void UpdateWinTimeLabel()
    {
        var timer = FindObjectOfType<GameTimer>();
        if (timer == null) return;

        float t = timer.GetLastRunTime();
        string formatted = FormatTime(t);

        if (tmpTimeText != null)
            tmpTimeText.text = formatted;

        if (uiTimeText != null)
            uiTimeText.text = formatted;
    }

    void UpdateWinKillLabel()
    {
        var counter = FindObjectOfType<KillCounter>();
        if (counter == null) return;

        int kills = counter.GetKills();
        string text = kills.ToString();

        if (tmpKillsText != null)
            tmpKillsText.text = text;

        if (uiKillsText != null)
            uiKillsText.text = text;
    }

    // --- Game Over UI ---
    public void ShowGameOver()
    {
        UpdateGameOverLabel();
        // removed obsolete MainMenu.started check, field no longer exists
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (pauseOnGameOver)
            Time.timeScale = 0f;
        
        
        // every time we show the game over panel we consider that the
        // player has just used up another attempt.  the counter is kept
        // across scene loads and will only reset when the application quits.
        // retry++;
        Debug.Log("Retry: " + retry);

        isGameOver = true;
        gameOver++;
        Debug.Log("GameOver: " + gameOver);
        CustomEvent exampleEvent = new CustomEvent("Game_Data")
        {
            {"FailureRate", gameOver},
            {"RetryRate", retry}
        };
        AnalyticsService.Instance.RecordEvent(exampleEvent);
    }

    public void HideGameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (pauseOnGameOver)
            Time.timeScale = 1f;

        // allow UI to be reused; clear flag so additional gameovers are handled
        isGameOver = false;
    }

    void UpdateGameOverLabel()
    {
        // Optionally show stats on game over; currently show kills
        var counter = FindObjectOfType<KillCounter>();
        if (counter == null) return;

        int kills = counter.GetKills();
        string text = "Kills: " + kills.ToString();

        // try to set into the same kill text fields if available
        if (tmpKillsText != null)
            tmpKillsText.text = text;

        if (uiKillsText != null)
            uiKillsText.text = text;
    }

    string FormatTime(float seconds)
    {
        int mins = Mathf.FloorToInt(seconds / 60f);
        float sec = seconds - mins * 60;
        return string.Format("{0:00}:{1:00.00}", mins, sec);
    }
}
