using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private int coinsPerClick = 1;
    [SerializeField] private double coinValue = 1;
    [SerializeField] private float force = 6f;
    [SerializeField] private float spread = 2.5f;

    [Header("Auto Spit")]
    [SerializeField] private int autoSpitAmount;
    [SerializeField] private float autoSpitInterval = 2f;

    private float autoSpitTimer;

    public int CoinsPerClick => coinsPerClick;
    public double CoinValue => coinValue;
    public float Force => force;
    public float Spread => spread;
    public int AutoSpitAmount => autoSpitAmount;
    public float AutoSpitInterval => autoSpitInterval;

    public double CoinsPerManualBurst => coinsPerClick * coinValue;
    public double AutoCoinsPerSecond => autoSpitAmount <= 0 || autoSpitInterval <= 0f
        ? 0
        : (autoSpitAmount * coinValue) / autoSpitInterval;

    private void Update()
    {
        if (autoSpitAmount <= 0 || autoSpitInterval <= 0f)
            return;

        autoSpitTimer += Time.deltaTime;
        while (autoSpitTimer >= autoSpitInterval)
        {
            autoSpitTimer -= autoSpitInterval;
            SpawnCoins(autoSpitAmount);
        }
    }

    public void SpawnBurst()
    {
        SpawnCoins();
    }

    public void SpawnCoins(int amountOverride = -1)
    {
        if (coinPrefab == null || spawnPoint == null)
        {
            Debug.LogWarning("CoinSpawner: falta coinPrefab o spawnPoint.", this);
            return;
        }

        int amount = amountOverride > 0 ? amountOverride : coinsPerClick;

        for (int i = 0; i < amount; i++)
        {
            var coin = Instantiate(coinPrefab, spawnPoint.position, Random.rotation);

            var pickup = coin.GetComponent<CoinPickup>();
            if (pickup == null)
                pickup = coin.GetComponentInChildren<CoinPickup>();

            if (pickup != null)
                pickup.SetValue(coinValue);

            var rb = coin.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 dir = spawnPoint.forward
                              + new Vector3(Random.Range(-1f, 1f), 0.6f, Random.Range(-1f, 1f)) * (spread * 0.3f);
                rb.AddForce(dir.normalized * force, ForceMode.Impulse);
                rb.AddTorque(Random.insideUnitSphere * 2f, ForceMode.Impulse);
            }
        }
    }

    public void AddCoinsPerClick(int add)
    {
        coinsPerClick = Mathf.Max(1, coinsPerClick + add);
    }

    public void MultiplyCoinValue(double multiplier)
    {
        if (multiplier <= 0)
            return;

        coinValue *= multiplier;
    }

    public void AddSpitForce(float add)
    {
        force = Mathf.Max(0.1f, force + add);
    }

    public void AddSpread(float add)
    {
        spread = Mathf.Max(0f, spread + add);
    }

    public void AddAutoSpitAmount(int add, float interval)
    {
        autoSpitAmount = Mathf.Max(0, autoSpitAmount + add);
        if (interval > 0f)
            autoSpitInterval = interval;
    }
}
