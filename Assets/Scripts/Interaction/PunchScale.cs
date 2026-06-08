using UnityEngine;

public class PunchScale : MonoBehaviour
{
    [SerializeField] private float punchAmount = 0.15f;
    [SerializeField] private float punchSpeed = 18f;

    Vector3 baseScale;
    float current;

    void Awake() => baseScale = transform.localScale;

    public void Punch()
    {
        current = 1f; // dispara el punch
    }

    void Update()
    {
        if (current <= 0f) return;

        current -= Time.deltaTime * punchSpeed;
        float s = 1f + Mathf.Sin((1f - current) * Mathf.PI) * punchAmount;
        transform.localScale = baseScale * s;

        if (current <= 0f)
            transform.localScale = baseScale;
    }
}