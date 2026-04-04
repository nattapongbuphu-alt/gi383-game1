using Unity.Services.Core;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Navigation : MonoBehaviour
{

    [Tooltip("Name of the main menu scene (used by Back button)")]
    public string mainMenuScene = "MainMenu";

    [SerializeField]
    [Tooltip("AudioSource for button click sound effects")]
    private AudioSource buttonSFX_Source;

    [SerializeField]
    [Tooltip("AudioSource for back button sound effects")]
    private AudioSource backSFX_Source;

    public TimeManager timeManager;
    public PlayerLight playerLight;

    public int retry = 0;
    public string fail;
    

    // public bool putButton = false;

    void Start()
    {
        Initialize();
    }

    private async void Initialize() 
    {
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
    }

    public void LoadScene(string sceneName)
    {   
        float finalTime = 0f;
        if(PlayerLight.isGameOver == true)
        {
            fail = "Ghost";
            // Debug.Log("Fail: " + fail);
            PlayerLight.isGameOver = false;
            finalTime = playerLight.d;
            // Debug.Log("Final Time: " + finalTime);
        }

        if (TimeManager.isGameOver == true)
        {
            fail = "TimeUp";
            // Debug.Log("Fail: " + fail);
            TimeManager.isGameOver = false;
            finalTime = timeManager.time;
        }

        Debug.Log("Fail: " + fail);
        Debug.Log("Final Time: " + finalTime); 

        int finalGameOver = UI.instance.gameOver;
        Debug.Log("Final GameOver: " + finalGameOver);
        retry++;
        Debug.Log("Retry: " + retry);
        CustomEvent exampleEvent = new CustomEvent("Game_Data07")
        {    
            {"RetryRate", retry},
            {"fail", fail},
            {"Time", finalTime},
            {"FailureRate", finalGameOver}
        };   
        AnalyticsService.Instance.RecordEvent(exampleEvent);

        PlayButtonSound();
        if (string.IsNullOrEmpty(sceneName)) return;
        SceneManager.LoadScene(sceneName);
    }

    public void LoadScene01(string sceneName)
    {  
        // if (UI.instance.isGameOver)
        // {
        //     UI.replay++;
        //     Debug.Log("Replay: " + UI.replay);
        // }
        // CustomEvent exampleEvent = new CustomEvent("Game_Data01")
        // {
        //     {"ReplayRate", UI.replay}
        // };
        // AnalyticsService.Instance.RecordEvent(exampleEvent);
        
        PlayButtonSound();
        if (string.IsNullOrEmpty(sceneName)) return;
        SceneManager.LoadScene(sceneName);
    }

    // Load the configured main menu scene
    public void LoadMainMenu()
    {
        PlayBackSound();
        if (string.IsNullOrEmpty(mainMenuScene)) return;
        SceneManager.LoadScene(mainMenuScene);
    }

    // Reload current scene (Restart)
    public void ReloadScene()
    {
        PlayButtonSound();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Load previous scene in Build Settings (index - 1)
    public void LoadPreviousScene()
    {
        PlayBackSound();
        int idx = SceneManager.GetActiveScene().buildIndex - 1;
        if (idx >= 0 && idx < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(idx);
    }

    // Play button sound effect
    private void PlayButtonSound()
    {
        if (buttonSFX_Source != null)
        {
            buttonSFX_Source.Stop();
            buttonSFX_Source.Play();
        }
    }

    // Play back button sound effect
    private void PlayBackSound()
    {
        if (backSFX_Source != null)
        {
            backSFX_Source.Stop();
            backSFX_Source.Play();
        }
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
