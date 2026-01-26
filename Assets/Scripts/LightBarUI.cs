using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LightBarUI : MonoBehaviour
{
    [Tooltip("Reference to PlayerLight (will try to FindObjectOfType if null)")]
    public PlayerLight playerLight;

    [Tooltip("UI Image using Fill (Image.Type = Filled)")]
    public Image fillImage;

    [Header("Optional Text")]
    public Text uiText;
    public TextMeshProUGUI tmpText;

    [Range(0f, 20f)]
    public float smoothSpeed = 8f;

    float currentFill = 0f;

    void Start()
    {
        if (playerLight == null)
            playerLight = FindObjectOfType<PlayerLight>();

        if (fillImage != null)
            currentFill = fillImage.fillAmount;
    }

    void Update()
    {
        if (playerLight == null) return;

        float target = 0f;
        if (playerLight.maxRadius > 0f)
            target = Mathf.Clamp01(playerLight.currentRadius / playerLight.maxRadius);

        currentFill = Mathf.Lerp(currentFill, target, Time.unscaledDeltaTime * smoothSpeed);

        if (fillImage != null)
            fillImage.fillAmount = currentFill;

        string txt = string.Format("{0:0.00}/{1:0.00}", playerLight.currentRadius, playerLight.maxRadius);
        if (tmpText != null)
            tmpText.text = txt;
        if (uiText != null)
            uiText.text = txt;
    }
}
