using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ButtonHoverBlink : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Hover")]
    public Color hoverColor = Color.yellow;

    [Header("Blink")]
    public Color blinkColor = Color.white;
    public float blinkInterval = 0.08f;
    public int blinkCount = 4;

    Text uiText;
    TextMeshProUGUI tmpText;
    Color originalColor = Color.white;
    Coroutine blinkCoroutine;

    void Awake()
    {
        // Try TextMeshPro first
        tmpText = GetComponentInChildren<TextMeshProUGUI>(true);
        if (tmpText == null)
            uiText = GetComponentInChildren<Text>(true);

        if (tmpText != null)
            originalColor = tmpText.color;
        else if (uiText != null)
            originalColor = uiText.color;
        else
            Debug.LogWarning("ButtonHoverBlink: No Text or TextMeshProUGUI found in children.", this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (blinkCoroutine != null) return;
        SetColor(hoverColor);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (blinkCoroutine != null) return;
        SetColor(originalColor);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (blinkCoroutine != null)
            CoroutineRunner.Instance.StopCoroutine(blinkCoroutine);
        blinkCoroutine = CoroutineRunner.Instance.StartCoroutine(BlinkRoutine());
    }

    IEnumerator BlinkRoutine()
    {
        for (int i = 0; i < blinkCount; i++)
        {
            SetColor(blinkColor);
            yield return new WaitForSeconds(blinkInterval);
            SetColor(originalColor);
            yield return new WaitForSeconds(blinkInterval);
        }
        blinkCoroutine = null;
    }

    void SetColor(Color c)
    {
        if (tmpText != null)
            tmpText.color = c;
        else if (uiText != null)
            uiText.color = c;
    }
}
