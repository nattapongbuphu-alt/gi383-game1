using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Tooltip("Panel that contains the main menu UI (enable/disable)")]
    public GameObject mainMenuPanel;

    [Tooltip("Name of the scene to load when starting the game. Must be added to Build Settings.")]
    public string firstSceneName = "GameScene";

    [Tooltip("AudioSource for button click sound effects")]
    public AudioSource buttonSFX_Source;

    void Start()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);

        Time.timeScale = 1f;
    }

    // Called by UI Start button
    public void StartGame()
    {
        PlayButtonSound();
        
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
        PlayButtonSound();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Optional: show menu again (useful for Pause -> MainMenu)
    public void ShowMenu()
    {
        PlayButtonSound();
        
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    // Play button sound effect with looping
    private void PlayButtonSound()
    {
        if (buttonSFX_Source != null)
        {
            buttonSFX_Source.PlayOneShot(buttonSFX_Source.clip);
        }
    }
}
