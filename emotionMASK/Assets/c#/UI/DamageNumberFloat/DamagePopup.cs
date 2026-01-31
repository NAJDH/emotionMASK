using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DamagePopup : MonoBehaviour
{
    public Text damageText;
    public float floatUpDistance = 60f;
    public float duration = 0.8f;
    public AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 1.2f, 1, 1f);
    public Color normalColor = Color.white;
    public Color criticalColor = new Color(1f, 0.85f, 0.2f);

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (damageText == null)
            damageText = GetComponentInChildren<Text>();
        // 防御：确保有默认曲线
        if (moveCurve == null || moveCurve.keys.Length == 0)
            moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        if (scaleCurve == null || scaleCurve.keys.Length == 0)
            scaleCurve = AnimationCurve.EaseInOut(0, 1.2f, 1, 1f);
    }

    public void SetText(string text, bool isCritical = false)
    {
        if (damageText == null) return;
        damageText.text = text;
        damageText.color = isCritical ? criticalColor : normalColor;
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        Vector2 startPos = rectTransform.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            // 位移（使用曲线）
            float y = moveCurve.Evaluate(t) * floatUpDistance;
            rectTransform.anchoredPosition = startPos + new Vector2(0f, y);

            // 缩放（punch 风格）
            float s = scaleCurve.Evaluate(t);
            rectTransform.localScale = Vector3.one * s;

            // 透明度渐隐
            Color c = damageText.color;
            c.a = 1f - t;
            damageText.color = c;

            yield return null;
        }

        Destroy(gameObject);
    }
}