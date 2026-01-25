using UnityEngine;

public class Exit : MonoBehaviour
{
    public GameObject gameOverUI; // ที่ UI จบเกม

    void OnTriggerEnter2D(Collider2D collision)
    {
        // ตรวจสอบว่าผู้เล่นชนกับ Exit
        if (collision.CompareTag("Player"))
        {
            // แสดง UI จบเกม
            if (gameOverUI != null)
            {
                gameOverUI.SetActive(true);
            }

            // หยุดเกม (ตัวเลือก)
            Time.timeScale = 0f;
        }
    }
}
