using UnityEngine;

public class CoinSource : MonoBehaviour
{
    [SerializeField] private CoinSpawner spawner;
    [SerializeField] private PunchScale punch;
    [SerializeField] private AudioSource sfxSource;

    private void Awake()
    {
        if (!punch) punch = GetComponent<PunchScale>();
    }

    public void Collect(EconomySystem economy)
    {
        if (punch != null) punch.Punch();
        if (sfxSource != null) sfxSource.Play();
        if (spawner != null) spawner.SpawnBurst();
        else Debug.LogWarning("CoinSource: spawner no asignado en el Inspector.", this);
    }
}