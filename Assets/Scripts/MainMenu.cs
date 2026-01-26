using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Tooltip("Panel that contains the main menu UI (enable/disable)")]
    public GameObject mainMenuPanel;

    [Tooltip("Name of the scene to load when starting the game. Must be added to Build Settings.")]
    public string firstSceneName = "GameScene";

    void Start()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);

        Time.timeScale = 1f;
    }

    // Called by UI Start button
    public void StartGame()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);

        if (!string.IsNullOrEmpty(firstSceneName))
        {
            SceneManager.LoadScene(firstSceneName);
        }
    }

    // Called by UI Quit button
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Optional: show menu again (useful for Pause -> MainMenu)
    public void ShowMenu()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);

        Time.timeScale = 0f;
    }
}
