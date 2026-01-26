using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    public GameObject winPanel;
    public string playerTag = "Player";
    public bool pauseOnWin = true;

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

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (pauseOnGameOver)
            Time.timeScale = 0f;
    }

    public void HideGameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (pauseOnGameOver)
            Time.timeScale = 1f;
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
