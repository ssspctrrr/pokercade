using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class TextAnimations : MonoBehaviour
{
    public void TransitionTextViaShake(TextMeshProUGUI textToShake, string newText)
    {
        StartCoroutine(TextShaker(textToShake, newText));
    }
    public IEnumerator TextShaker(TextMeshProUGUI textToShake, string newText, float magnitude = 25, float duration = 0.5f)
    {
        RectTransform textTransform = textToShake.rectTransform;
        Vector3 originalPos = textTransform.anchoredPosition;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = (Mathf.PerlinNoise(Time.time * 20f, 0f) - 0.5f) * magnitude;
            float y = (Mathf.PerlinNoise(0f, Time.time * 20f) - 0.5f) * magnitude;

            textTransform.anchoredPosition = originalPos + new Vector3(x, y, 0);

            if (elapsed >= duration / 2)
                textToShake.text = newText;

            elapsed += Time.deltaTime;
            yield return null;
        }

        textTransform.anchoredPosition = originalPos;
    }
}
