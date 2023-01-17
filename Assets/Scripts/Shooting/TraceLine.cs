using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TraceLine : MonoBehaviour
{
    [SerializeField] float lifetime = 1;
    [SerializeField] float targetAlpha;
    float timeleft = 0;

    LineRenderer line;
    Color start;
    Color end;
    Color targetStart;
    Color targetEnd;

    void Awake()
    {
        timeleft = lifetime;

        line = GetComponent<LineRenderer>();
        start = line.startColor;
        end = line.endColor;

        targetStart = start;
        targetStart.a = targetAlpha;
        targetEnd = end;
        targetEnd.a = targetAlpha;
    }

    void Update()
    {
        timeleft -= Time.deltaTime;

        if (timeleft <= 0)
            Destroy(gameObject);

        float t = lifetime - timeleft;
        line.startColor = Color.Lerp(start, targetStart, t);
        line.endColor = Color.Lerp(end, targetEnd, t);
    }
}