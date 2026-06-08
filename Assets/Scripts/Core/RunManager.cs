using UnityEngine;

public class RunManager : MonoBehaviour
{
    [SerializeField] private float runSeconds = 2f * 60f * 60f;
    [SerializeField] private double goalCoins = 100000;
    [SerializeField] private EconomySystem economy;
    [SerializeField] private DoorController doorController;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private bool pauseOnEnd = true;

    public float RemainingSeconds { get; private set; }
    public bool IsRunning { get; private set; } = true;
    public bool GoalReached { get; private set; }
    public bool HasWon { get; private set; }
    public bool HasLost { get; private set; }
    public double GoalCoins => goalCoins;
    public double GoalProgress => goalCoins <= 0
        ? 1
        : economy == null
            ? 0
        : System.Math.Min(1, economy.TotalCoinsEarned / goalCoins);

    private void Start()
    {
        if (!economy) economy = FindAnyObjectByType<EconomySystem>();
        if (!doorController) doorController = FindAnyObjectByType<DoorController>();

        if (!doorController)
        {
            var door = GameObject.Find("Door");
            if (door != null)
                doorController = door.GetComponent<DoorController>() ?? door.AddComponent<DoorController>();
        }

        if (doorController)
            doorController.SetRunManager(this);

        if (winPanel) winPanel.SetActive(false);
        if (losePanel) losePanel.SetActive(false);

        RemainingSeconds = runSeconds;
    }

    private void Update()
    {
        if (!IsRunning)
            return;

        if (!GoalReached && economy != null && economy.TotalCoinsEarned >= goalCoins)
        {
            GoalReached = true;
            doorController?.SetUnlocked(true);
            Debug.Log("GOAL REACHED: door unlocked.");
        }

        RemainingSeconds -= Time.deltaTime;
        if (RemainingSeconds <= 0f)
        {
            RemainingSeconds = 0f;
            if (!GoalReached)
                Lose();
        }
    }

    public void Win()
    {
        if (HasWon || HasLost)
            return;

        HasWon = true;
        IsRunning = false;
        if (winPanel) winPanel.SetActive(true);
        if (pauseOnEnd) Time.timeScale = 0f;
        Debug.Log("RUN WON!");
    }

    public void Lose()
    {
        if (HasWon || HasLost)
            return;

        HasLost = true;
        IsRunning = false;
        if (losePanel) losePanel.SetActive(true);
        if (pauseOnEnd) Time.timeScale = 0f;
        Debug.Log("TIME UP!");
    }

    public string GetFormattedTime()
    {
        int total = Mathf.CeilToInt(RemainingSeconds);
        int h = total / 3600;
        int m = (total % 3600) / 60;
        int s = total % 60;
        return $"{h:00}:{m:00}:{s:00}";
    }
}
