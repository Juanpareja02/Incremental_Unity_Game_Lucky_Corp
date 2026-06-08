using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] private double value = 1;
    [SerializeField] private float lifeTime = 25f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void PickUp(EconomySystem economy)
    {
        if (economy == null)
        {
            Debug.LogWarning("CoinPickup: no EconomySystem assigned.", this);
            return;
        }

        economy.AddCoins(value);
        Destroy(gameObject);
    }

    public void SetValue(double v)
    {
        value = v;
    }
}
