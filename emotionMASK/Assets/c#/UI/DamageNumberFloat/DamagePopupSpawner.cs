using UnityEngine;
using UnityEngine.UI;

public class DamagePopupSpawner : MonoBehaviour
{
    public static DamagePopupSpawner Instance { get; private set; }

    [Tooltip("目标 Canvas（优先使用），若为空会尝试自动查找或创建 ScreenSpace-Overlay Canvas")]
    public Canvas targetCanvas;

    [Tooltip("指向 DamagePopup 预制体（必须包含 DamagePopup 脚本）")]
    public GameObject popupPrefab;

    [Tooltip("额外屏幕偏移（像素）")]
    public Vector2 screenOffset = Vector2.zero;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        if (targetCanvas == null)
            targetCanvas = FindObjectOfType<Canvas>();

        if (targetCanvas == null)
        {
            var go = new GameObject("DamagePopupCanvas");
            var canvas = go.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            go.AddComponent<CanvasScaler>();
            go.AddComponent<GraphicRaycaster>();
            targetCanvas = canvas;
        }

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 在世界坐标 worldPosition 上显示伤害数字（使用 unscaled time 动画）
    /// </summary>
    public void ShowDamage(Vector3 worldPosition, float damage, bool isCritical = false)
    {
        if (popupPrefab == null || targetCanvas == null) return;

        // 计算屏幕点并转换为 Canvas 本地点
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);
        RectTransform canvasRect = targetCanvas.transform as RectTransform;
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, targetCanvas.worldCamera, out localPoint);

        GameObject inst = Instantiate(popupPrefab, targetCanvas.transform);
        RectTransform rt = inst.GetComponent<RectTransform>();
        rt.anchoredPosition = localPoint + screenOffset;
        rt.localScale = Vector3.one;

        var popup = inst.GetComponent<DamagePopup>();
        if (popup != null)
            popup.SetText(((int)damage).ToString(), isCritical);
    }
}