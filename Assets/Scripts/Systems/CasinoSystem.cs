using UnityEngine;

public class CasinoSystem : MonoBehaviour
{
    [SerializeField] private EconomySystem economy;

    [Range(0f, 1f)]
    [SerializeField] private float winChance = 0.48f;

    public string LastResult { get; private set; }

    private void Awake()
    {
        if (!economy) economy = FindAnyObjectByType<EconomySystem>();
    }

    public bool TryDoubleOrNothing(double bet)
    {
        if (economy == null || bet <= 0)
            return false;

        if (!economy.TrySpend(bet))
        {
            LastResult = "sin monedas suficientes";
            return false;
        }

        bool won = Random.value < winChance;
        if (won)
        {
            double payout = bet * 2;
            economy.AddCoins(payout);
            LastResult = $"ganaste {Format(payout)}";
        }
        else
        {
            LastResult = $"perdiste {Format(bet)}";
        }

        return true;
    }

    private static string Format(double v)
    {
        if (v < 1000) return v.ToString("0");
        if (v < 1_000_000) return (v / 1000d).ToString("0.0") + "K";
        if (v < 1_000_000_000) return (v / 1_000_000d).ToString("0.0") + "M";
        return (v / 1_000_000_000d).ToString("0.0") + "B";
    }
}
