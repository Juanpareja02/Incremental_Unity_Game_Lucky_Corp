using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorShopUI : MonoBehaviour
{
    [SerializeField] private GeneratorShopSystem shop;
    [SerializeField] private EconomySystem economy;
    [SerializeField] private CasinoSystem casino;
    [SerializeField] private Transform container;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private double casinoBetAmount = 100;

    private void Awake()
    {
        if (!shop) shop = FindAnyObjectByType<GeneratorShopSystem>();
        if (!economy) economy = FindAnyObjectByType<EconomySystem>();
        if (!casino) casino = FindAnyObjectByType<CasinoSystem>();
        if (!casino) casino = gameObject.AddComponent<CasinoSystem>();
    }

    private void Start()
    {
        Build();
    }

    private void Build()
    {
        foreach (Transform child in container)
            Destroy(child.gameObject);

        foreach (var def in shop.Generators)
            BuildUpgradeRow(def);

        BuildCasinoRow();
    }

    private void BuildUpgradeRow(GeneratorDef def)
    {
        var go = Instantiate(itemPrefab, container);
        var btn = go.GetComponent<Button>();
        var label = go.GetComponentInChildren<TMP_Text>();
        ApplyRowBaseStyle(go, btn, label, false);

        void Refresh()
        {
            int lvl = shop.GetLevel(def);
            double cost = shop.GetCost(def);

            string levelText = def.HasMaxLevel ? $"Lv {lvl}/{def.maxLevel}" : $"Lv {lvl}";
            string status = shop.IsMaxed(def) ? "MAX" : $"Coste: {Format(cost)}";
            label.text = $"<b>{def.displayName}</b>  <color=#8BE9FD>{levelText}</color>\n<color=#E6EDF3>{def.GetEffectLabel()}</color>  <color=#FFD166>{status}</color>";

            bool canBuy = !shop.IsMaxed(def) && economy.Coins >= cost;
            btn.interactable = canBuy;
            ApplyRowState(go, canBuy, shop.IsMaxed(def), false);

            if (!canBuy && !shop.IsMaxed(def))
            {
                double missing = cost - economy.Coins;
                label.text += $"  <color=#FF6B6B>Faltan {Format(missing)}</color>";
            }
        }

        Refresh();

        btn.onClick.AddListener(() =>
        {
            if (shop.TryBuy(def))
                Refresh();
        });

        go.AddComponent<AutoRefresh>().Init(Refresh);
    }

    private void BuildCasinoRow()
    {
        if (casino == null || itemPrefab == null || container == null)
            return;

        var go = Instantiate(itemPrefab, container);
        var btn = go.GetComponent<Button>();
        var label = go.GetComponentInChildren<TMP_Text>();
        ApplyRowBaseStyle(go, btn, label, true);

        void Refresh()
        {
            bool canBet = economy != null && economy.Coins >= casinoBetAmount;
            btn.interactable = canBet;
            ApplyRowState(go, canBet, false, true);

            string result = string.IsNullOrEmpty(casino.LastResult)
                ? "riesgo 50%, paga 2x"
                : casino.LastResult;

            label.text = $"<b>Casino: Doble o nada</b>  <color=#FF9F1C>Apuesta {Format(casinoBetAmount)}</color>\n<color=#E6EDF3>{result}</color>";
            if (!canBet && economy != null)
                label.text += $"  <color=#FF6B6B>Faltan {Format(casinoBetAmount - economy.Coins)}</color>";
        }

        Refresh();

        btn.onClick.AddListener(() =>
        {
            casino.TryDoubleOrNothing(casinoBetAmount);
            Refresh();
        });

        go.AddComponent<AutoRefresh>().Init(Refresh);
    }

    private static void ApplyRowBaseStyle(GameObject row, Button button, TMP_Text label, bool casinoRow)
    {
        var layout = row.GetComponent<LayoutElement>();
        if (layout)
        {
            layout.minHeight = 58f;
            layout.preferredHeight = 58f;
            layout.flexibleWidth = 1f;
        }

        var rect = row.GetComponent<RectTransform>();
        if (rect)
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, 58f);

        var image = row.GetComponent<Image>();
        if (image)
            image.color = casinoRow ? new Color(0.22f, 0.14f, 0.08f, 0.92f) : new Color(0.09f, 0.11f, 0.13f, 0.92f);

        if (button)
        {
            ColorBlock colors = button.colors;
            colors.normalColor = Color.white;
            colors.highlightedColor = casinoRow ? new Color(1f, 0.75f, 0.36f) : new Color(0.65f, 0.95f, 1f);
            colors.pressedColor = casinoRow ? new Color(0.85f, 0.42f, 0.1f) : new Color(0.25f, 0.65f, 0.75f);
            colors.selectedColor = colors.highlightedColor;
            colors.disabledColor = new Color(0.34f, 0.36f, 0.38f, 0.75f);
            colors.fadeDuration = 0.08f;
            button.colors = colors;
        }

        if (label)
        {
            label.textWrappingMode = TextWrappingModes.Normal;
            label.overflowMode = TextOverflowModes.Ellipsis;
            label.fontSize = 15f;
            label.alignment = TextAlignmentOptions.MidlineLeft;
            label.margin = new Vector4(20f, 0f, 18f, 0f);
            label.color = Color.white;
        }

        EnsureAccentBar(row.transform, casinoRow);
    }

    private static void ApplyRowState(GameObject row, bool canBuy, bool maxed, bool casinoRow)
    {
        var image = row.GetComponent<Image>();
        if (!image)
            return;

        if (maxed)
            image.color = new Color(0.08f, 0.18f, 0.12f, 0.95f);
        else if (canBuy)
            image.color = casinoRow ? new Color(0.27f, 0.16f, 0.07f, 0.95f) : new Color(0.08f, 0.15f, 0.17f, 0.95f);
        else
            image.color = new Color(0.1f, 0.11f, 0.12f, 0.72f);
    }

    private static void EnsureAccentBar(Transform row, bool casinoRow)
    {
        if (row.Find("AccentBar"))
            return;

        var bar = new GameObject("AccentBar", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        bar.transform.SetParent(row, false);
        var rect = bar.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0f, 0f);
        rect.anchorMax = new Vector2(0f, 1f);
        rect.pivot = new Vector2(0f, 0.5f);
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = new Vector2(6f, 0f);

        var image = bar.GetComponent<Image>();
        image.color = casinoRow ? new Color(1f, 0.53f, 0.12f) : new Color(0.33f, 0.91f, 1f);
        image.raycastTarget = false;
    }

    private static string Format(double v)
    {
        if (v < 1000) return v.ToString("0");
        if (v < 1_000_000) return (v / 1000d).ToString("0.0") + "K";
        if (v < 1_000_000_000) return (v / 1_000_000d).ToString("0.0") + "M";
        return (v / 1_000_000_000d).ToString("0.0") + "B";
    }

    private class AutoRefresh : MonoBehaviour
    {
        private System.Action refresh;

        public void Init(System.Action r)
        {
            refresh = r;
        }

        private void Update()
        {
            refresh?.Invoke();
        }
    }
}
