using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    [SerializeField] private EconomySystem economy;

    public void Collect(double amount)
    {
        economy.AddCoins(amount);
    }
}