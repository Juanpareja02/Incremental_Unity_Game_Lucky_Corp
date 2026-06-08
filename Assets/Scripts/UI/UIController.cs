using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private EconomySystem economy;
    [SerializeField] private RunManager runManager;
    [SerializeField] private CoinSpawner coinSpawner;

    [Header("UI")]
    [SerializeField] private TMP_Text coinsText;
    [SerializeField] private TMP_Text statsText;
    [SerializeField] private TMP_Text timerText;

    private void Awake()
    {
        if (!economy) economy = FindAnyObjectByType<EconomySystem>();
        if (!runManager) runManager = FindAnyObjectByType<RunManager>();
        if (!coinSpawner) coinSpawner = FindAnyObjectByType<CoinSpawner>();
    }

    private void Update()
    {
        if (economy)
        {
            if (coinsText)
                coinsText.text = $"MONEDAS\n<color=#FFD166>{Format(economy.Coins)}</color>";

            if (statsText)
            {
                if (coinSpawner)
                {
                    statsText.text = $"NUCLEO\nClick x{coinSpawner.CoinsPerClick}  |  Valor {Format(coinSpawner.CoinValue)}  |  Auto {Format(coinSpawner.AutoCoinsPerSecond)}/s";
                }
                else
                {
                    statsText.text = $"NUCLEO\nCPS {Format(economy.CoinsPerSecond)}  |  DPC {Format(economy.CoinsPerClick)}";
                }

                if (runManager)
                {
                    int percent = Mathf.RoundToInt((float)(runManager.GoalProgress * 100));
                    statsText.text += $"\nMeta {percent}%";
                }
            }
        }

        if (runManager && timerText)
        {
            timerText.text = runManager.GetFormattedTime();
            timerText.color = runManager.RemainingSeconds <= 300f
                ? new Color(1f, 0.32f, 0.22f)
                : Color.white;
        }
    }

    private static string Format(double v)
    {
        if (v < 1000) return v.ToString("0");
        if (v < 1_000_000) return (v / 1000d).ToString("0.0") + "K";
        if (v < 1_000_000_000) return (v / 1_000_000d).ToString("0.0") + "M";
        return (v / 1_000_000_000d).ToString("0.0") + "B";
    }
}
