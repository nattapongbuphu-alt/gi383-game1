using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerLight : MonoBehaviour
{
    public Light2D light2D;
    public float minRadius = 1.5f;
    public float maxRadius = 6f;
    public float currentRadius = 2f;

    void Start()
    {
        UpdateLight();
    }

    public void AddLight(float value)
    {
        currentRadius = Mathf.Clamp(currentRadius + value, minRadius, maxRadius);
        UpdateLight();
    }

    public void UseLight(float value)
    {
        currentRadius = Mathf.Clamp(currentRadius - value, minRadius, maxRadius);
        UpdateLight();
    }

    void UpdateLight()
    {
        light2D.pointLightOuterRadius = currentRadius;
    }
}
