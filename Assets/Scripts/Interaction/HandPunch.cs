using UnityEngine;

public class HandPunch : MonoBehaviour
{
    [SerializeField] float distance = 0.07f;
    [SerializeField] float speed = 22f;

    Vector3 startPos;
    float t;
    bool active;

    void Awake() => startPos = transform.localPosition;

    public void Play()
    {
        t = 0f;
        active = true;
    }

    void Update()
    {
        if (!active) return;

        t += Time.deltaTime * speed;
        float s = Mathf.Sin(t * Mathf.PI); // 0->1->0
        transform.localPosition = startPos + Vector3.forward * (s * distance);

        if (t >= 1f)
        {
            transform.localPosition = startPos;
            active = false;
        }
    }
}