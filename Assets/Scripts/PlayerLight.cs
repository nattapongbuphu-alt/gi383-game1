using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerLight : MonoBehaviour
{
    public Light2D light2D;

    public float maxRadius = 6f;
    public float minRadius = 0f;   // 0 = ตาย
    public float currentRadius = 3f;

    void Start()
    {
        UpdateLight();
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
        Debug.Log("GAME OVER");
        Time.timeScale = 0f; // หยุดเกม
        // ต่อไปค่อยใส่ UI Game Over
    }

    public bool HasEnoughLight(float cost)
    {
        return currentRadius - cost > 0.01f;
    }
}
