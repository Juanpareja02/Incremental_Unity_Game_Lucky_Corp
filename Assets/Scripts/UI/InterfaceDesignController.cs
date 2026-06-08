using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(100)]
public class InterfaceDesignController : MonoBehaviour
{
    private readonly Color panelColor = new Color(0.03f, 0.04f, 0.05f, 0.82f);
    private readonly Color shopColor = new Color(0.04f, 0.055f, 0.07f, 0.96f);
    private readonly Color accentCyan = new Color(0.33f, 0.91f, 1f, 1f);
    private readonly Color accentGold = new Color(1f, 0.82f, 0.38f, 1f);

    private RunManager runManager;
    private Image progressFill;
    private TMP_Text progressLabel;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Bootstrap()
    {
        if (FindAnyObjectByType<InterfaceDesignController>() != null)
            return;

        var go = new GameObject("InterfaceDesignController");
        DontDestroyOnLoad(go);
        go.AddComponent<InterfaceDesignController>();
    }

    private void Start()
    {
        runManager = FindAnyObjectByType<RunManager>();

        var canvas = FindObject("Canvas");
        if (!canvas)
            return;

        StyleHud(canvas.transform);
        StyleShop();
        StyleCrosshair();
        BuildProgressBar(canvas.transform);
    }

    private void Update()
    {
        if (!runManager || !progressFill || !progressLabel)
            return;

        float progress = Mathf.Clamp01((float)runManager.GoalProgress);
        progressFill.fillAmount = progress;
        progressLabel.text = $"META {Mathf.RoundToInt(progress * 100f)}%";
    }

    private void StyleHud(Transform canvas)
    {
        var hudPanel = FindObject("HudPanel");
        if (!hudPanel)
            hudPanel = CreatePanel("HudPanel", canvas, panelColor);

        var rect = hudPanel.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(1f, 1f);
        rect.anchorMax = new Vector2(1f, 1f);
        rect.pivot = new Vector2(1f, 1f);
        rect.anchoredPosition = new Vector2(-24f, -24f);
        rect.sizeDelta = new Vector2(430f, 178f);

        var layout = hudPanel.GetComponent<VerticalLayoutGroup>() ?? hudPanel.AddComponent<VerticalLayoutGroup>();
        layout.padding = new RectOffset(18, 18, 14, 14);
        layout.spacing = 8f;
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = false;

        MoveHudText("CoinsText", hudPanel.transform, 26f, 50f);
        MoveHudText("TimerText", hudPanel.transform, 24f, 42f);
        MoveHudText("StatsText", hudPanel.transform, 16f, 58f);
    }

    private void MoveHudText(string name, Transform parent, float fontSize, float height)
    {
        var go = FindObject(name);
        if (!go)
            return;

        go.transform.SetParent(parent, false);
        var text = go.GetComponent<TMP_Text>();
        if (text)
        {
            text.fontSize = fontSize;
            text.textWrappingMode = TextWrappingModes.Normal;
            text.alignment = TextAlignmentOptions.MidlineLeft;
            text.color = Color.white;
        }

        var layout = go.GetComponent<LayoutElement>() ?? go.AddComponent<LayoutElement>();
        layout.preferredHeight = height;
        layout.flexibleWidth = 1f;

        var rect = go.GetComponent<RectTransform>();
        rect.localScale = Vector3.one;
        rect.anchorMin = new Vector2(0f, 1f);
        rect.anchorMax = new Vector2(1f, 1f);
        rect.pivot = new Vector2(0.5f, 1f);
        rect.sizeDelta = new Vector2(0f, height);
    }

    private void StyleShop()
    {
        var overlay = FindObject("ShopPanel");
        if (overlay)
        {
            var image = overlay.GetComponent<Image>();
            if (image)
                image.color = new Color(0f, 0f, 0f, 0.68f);
        }

        var window = FindObject("ShopWindow");
        if (window)
        {
            var rect = window.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(820f, 640f);

            var image = window.GetComponent<Image>();
            if (image)
                image.color = shopColor;

            var outline = window.GetComponent<Outline>() ?? window.AddComponent<Outline>();
            outline.effectColor = new Color(0.33f, 0.91f, 1f, 0.38f);
            outline.effectDistance = new Vector2(2f, -2f);
        }

        StyleText("ShopTitle", "MACHINE UPGRADES", 34f, accentGold, TextAlignmentOptions.Center);
        StyleText("HintText", "TAB cerrar  |  Click comprar  |  Casino al final", 15f, new Color(0.75f, 0.82f, 0.88f), TextAlignmentOptions.Center);

        var container = FindObject("ItemContainer");
        if (container)
        {
            var rect = container.GetComponent<RectTransform>();
            rect.offsetMin = new Vector2(0f, 64f);
            rect.offsetMax = new Vector2(0f, -76f);

            var layout = container.GetComponent<VerticalLayoutGroup>();
            if (layout)
            {
                layout.padding = new RectOffset(24, 24, 12, 12);
                layout.spacing = 8f;
            }
        }
    }

    private void StyleCrosshair()
    {
        var crosshair = FindObject("Crosshair");
        if (!crosshair)
            return;

        var rect = crosshair.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(6f, 6f);

        var image = crosshair.GetComponent<Image>();
        if (image)
        {
            image.color = new Color(1f, 1f, 1f, 0.92f);
            image.raycastTarget = false;
        }

        CreateCrosshairLine(crosshair.transform, "Top", new Vector2(2f, 12f), new Vector2(0f, 15f));
        CreateCrosshairLine(crosshair.transform, "Bottom", new Vector2(2f, 12f), new Vector2(0f, -15f));
        CreateCrosshairLine(crosshair.transform, "Left", new Vector2(12f, 2f), new Vector2(-15f, 0f));
        CreateCrosshairLine(crosshair.transform, "Right", new Vector2(12f, 2f), new Vector2(15f, 0f));
    }

    private void BuildProgressBar(Transform canvas)
    {
        var frame = FindObject("RunProgressBar");
        if (!frame)
            frame = CreatePanel("RunProgressBar", canvas, new Color(0.02f, 0.025f, 0.03f, 0.82f));

        var frameRect = frame.GetComponent<RectTransform>();
        frameRect.anchorMin = new Vector2(0.5f, 0f);
        frameRect.anchorMax = new Vector2(0.5f, 0f);
        frameRect.pivot = new Vector2(0.5f, 0f);
        frameRect.anchoredPosition = new Vector2(0f, 24f);
        frameRect.sizeDelta = new Vector2(520f, 34f);

        progressFill = FindImage(frame.transform, "Fill");
        if (!progressFill)
        {
            var fill = new GameObject("Fill", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            fill.transform.SetParent(frame.transform, false);
            progressFill = fill.GetComponent<Image>();
        }

        var fillRect = progressFill.GetComponent<RectTransform>();
        fillRect.anchorMin = new Vector2(0f, 0f);
        fillRect.anchorMax = new Vector2(1f, 1f);
        fillRect.offsetMin = new Vector2(4f, 4f);
        fillRect.offsetMax = new Vector2(-4f, -4f);
        progressFill.color = accentCyan;
        progressFill.type = Image.Type.Filled;
        progressFill.fillMethod = Image.FillMethod.Horizontal;
        progressFill.fillOrigin = (int)Image.OriginHorizontal.Left;
        progressFill.fillAmount = 0f;
        progressFill.raycastTarget = false;

        progressLabel = frame.GetComponentInChildren<TMP_Text>();
        if (!progressLabel)
        {
            var label = new GameObject("Label", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
            label.transform.SetParent(frame.transform, false);
            progressLabel = label.GetComponent<TMP_Text>();
        }

        var labelRect = progressLabel.GetComponent<RectTransform>();
        labelRect.anchorMin = Vector2.zero;
        labelRect.anchorMax = Vector2.one;
        labelRect.offsetMin = Vector2.zero;
        labelRect.offsetMax = Vector2.zero;
        progressLabel.fontSize = 15f;
        progressLabel.alignment = TextAlignmentOptions.Center;
        progressLabel.color = Color.white;
        progressLabel.raycastTarget = false;
    }

    private void StyleText(string name, string value, float size, Color color, TextAlignmentOptions alignment)
    {
        var go = FindObject(name);
        if (!go)
            return;

        var text = go.GetComponent<TMP_Text>();
        if (!text)
            return;

        text.text = value;
        text.fontSize = size;
        text.color = color;
        text.alignment = alignment;
        text.textWrappingMode = TextWrappingModes.Normal;
    }

    private void CreateCrosshairLine(Transform parent, string name, Vector2 size, Vector2 position)
    {
        if (parent.Find(name))
            return;

        var go = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        go.transform.SetParent(parent, false);
        var rect = go.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;

        var image = go.GetComponent<Image>();
        image.color = new Color(1f, 1f, 1f, 0.7f);
        image.raycastTarget = false;
    }

    private GameObject CreatePanel(string name, Transform parent, Color color)
    {
        var go = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        go.transform.SetParent(parent, false);
        var image = go.GetComponent<Image>();
        image.color = color;
        image.raycastTarget = false;
        return go;
    }

    private static GameObject FindObject(string name)
    {
        var transform = FindTransform(name);
        return transform ? transform.gameObject : null;
    }

    private static Transform FindTransform(string name)
    {
        var all = Resources.FindObjectsOfTypeAll<Transform>();
        for (int i = 0; i < all.Length; i++)
        {
            if (all[i].name == name && all[i].gameObject.scene.IsValid())
                return all[i];
        }

        return null;
    }

    private static Image FindImage(Transform parent, string name)
    {
        var child = parent.Find(name);
        return child ? child.GetComponent<Image>() : null;
    }
}
