using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public float timeLimit = 300f; // เวลาทั้งหมด (วินาที)
    public TextMeshProUGUI timerText;

    private float currentTime;
    private bool isGameOver = false;

    void Start()
    {
        currentTime = timeLimit;
    }

    void Update()
    {
        if (isGameOver) return;

        currentTime -= Time.deltaTime;
        UpdateUI();

        if (currentTime <= 0f)
        {
            GameOver();
        }
    }

    void UpdateUI()
    {
        if (currentTime < 30f)
        {
            timerText.color = Color.red;
        }

        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }
    

    void GameOver()
    {
        isGameOver = true;
        currentTime = 0;
        timerText.text = "Time: 0";

        Debug.Log("GAME OVER - TIME UP");
        Time.timeScale = 0f; // หยุดเกม
    }
}
