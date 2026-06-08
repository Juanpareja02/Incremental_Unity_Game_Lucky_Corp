using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private float lifetime = 0.8f;
    [SerializeField] private float floatSpeed = 40f;

    float t;

    void Awake()
    {
        if (!text) text = GetComponent<TMP_Text>();
    }

    public void Init(string value)
    {
        text.text = value;
        t = 0f;
    }

    void Update()
    {
        t += Time.deltaTime;

        // Subir
        transform.position += Vector3.up * (floatSpeed * Time.deltaTime);

        // Fade
        float a = Mathf.Lerp(1f, 0f, t / lifetime);
        var c = text.color;
        c.a = a;
        text.color = c;

        if (t >= lifetime) Destroy(gameObject);
    }
}