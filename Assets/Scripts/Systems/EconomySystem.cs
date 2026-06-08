using UnityEngine;

public class EconomySystem : MonoBehaviour
{
    public double Coins { get; private set; }
    public double TotalCoinsEarned { get; private set; }

    // Legacy stats kept so older scene references still work.
    public double CoinsPerSecond { get; private set; } = 0;
    public double CoinsPerClick { get; private set; } = 1;

    public void AddCoins(double amount)
    {
        if (amount > 0)
            TotalCoinsEarned += amount;

        Coins += amount;
        if (Coins < 0)
            Coins = 0;
    }

    public bool TrySpend(double amount)
    {
        if (amount <= 0)
            return true;

        if (Coins < amount)
            return false;

        Coins -= amount;
        return true;
    }

    public void SetCoinsPerSecond(double value)
    {
        CoinsPerSecond = value < 0 ? 0 : value;
    }

    public void SetCoinsPerClick(double value)
    {
        CoinsPerClick = value < 0 ? 0 : value;
    }

    public void SetCoins(double value)
    {
        Coins = value < 0 ? 0 : value;
    }

    public void SetTotalCoinsEarned(double value)
    {
        TotalCoinsEarned = value < 0 ? 0 : value;
    }

    private void Update()
    {
        if (CoinsPerSecond > 0)
            AddCoins(CoinsPerSecond * Time.deltaTime);
    }
}
