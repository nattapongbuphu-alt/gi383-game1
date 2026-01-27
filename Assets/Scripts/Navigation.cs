using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Navigation : MonoBehaviour
{
    [Tooltip("Name of the main menu scene (used by Back button)")]
    public string mainMenuScene = "MainMenu";

    // Load scene by name (assignable from Button OnClick)
    public void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName)) return;
        SceneManager.LoadScene(sceneName);
    }

    // Load the configured main menu scene
    public void LoadMainMenu()
    {
        if (string.IsNullOrEmpty(mainMenuScene)) return;
        SceneManager.LoadScene(mainMenuScene);
    }

    // Reload current scene (Restart)
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Load previous scene in Build Settings (index - 1)
    public void LoadPreviousScene()
    {
        int idx = SceneManager.GetActiveScene().buildIndex - 1;
        if (idx >= 0 && idx < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(idx);
    }

    // Quit application (works in editor and build)
    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
