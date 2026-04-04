using Unity.Services.Core;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerLight : MonoBehaviour
{
    public string fail = "Ghost";

    public Light2D light2D;

    public float maxRadius = 6f;
    public float minRadius = 0f;   // 0 = ตาย
    public float currentRadius = 3f;
    public float d;

    public static bool isGameOver = false;

    void Start()
    {
        UpdateLight();
        Initialize();
    }

    private async void Initialize() 
    {
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
    }

    public void AddLight(float value)
    {
        currentRadius = Mathf.Clamp(currentRadius + value, minRadius, maxRadius);
        UpdateLight();
    }

    public void TakeDamage(float value)
    {
        currentRadius = Mathf.Clamp(currentRadius - value, minRadius, maxRadius);
        UpdateLight();

        if (currentRadius <= 0f)
        {
            Die();
        }
    }

    void UpdateLight()
    {
        light2D.pointLightOuterRadius = currentRadius;
    }

    void Die()
    {
        d = Time.time - TimeManager.instance.timeCount;
        // Debug.Log("Time: " + d);
        
        // Debug.Log("GAME OVER");

        isGameOver = true;

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

    public bool HasEnoughLight(float cost)
    {
        return currentRadius - cost > 0.01f;
    }
}
