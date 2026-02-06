using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Rope : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    

    [Header("Rope Settings")]
    [SerializeField] private int segments = 25;
    [SerializeField] private float waveAmplitude = 0.2f;
    [SerializeField] private float waveFrequency = 2f;
    [SerializeField] private float waveSpeed = 5f;

    private LineRenderer line;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = segments;
    }

    void Update()
    {
        if(ThrowLasso.Instance.hasThrown && !ThrowLasso.Instance.recallRope){ DrawRope(); }
        else if (ThrowLasso.Instance.hasThrown && ThrowLasso.Instance.recallRope) { RecallRope(); }
        else { line.enabled = false; }
    }

    void DrawRope()
    {
        line.enabled = true;
        
        Vector3 start = startPoint.position;
        Vector3 end = endPoint.position;

        Vector3 dir = (end - start).normalized;

        // Deux axes perpendiculaires
        Vector3 side = Vector3.Cross(Vector3.up, dir).normalized;
        Vector3 up = Vector3.Cross(dir, side).normalized;

        for (int i = 0; i < segments; i++)
        {
            float t = i / (float)(segments - 1);

            Vector3 point = Vector3.Lerp(start, end, t);

            // Onde de base
            float wave = Mathf.Sin(t * waveFrequency + Time.time * waveSpeed)
                       * waveAmplitude
                       * Mathf.Sin(t * Mathf.PI);

            // Direction pseudo-al�atoire, douce
            float noise = Mathf.PerlinNoise(t * 2f, Time.time * 0.5f);
            float angle = noise * Mathf.PI * 2f;

            Vector3 randomDir =
                Mathf.Cos(angle) * side +
                Mathf.Sin(angle) * up;

            point += randomDir * wave;

            line.SetPosition(i, point);
        }
    }

    void RecallRope()
    {
        Vector3 start = startPoint.position;
        Vector3 end = endPoint.position;

        for (int i = 0; i < segments; i++)
        {
            float t = i / (float)(segments - 1);
            Vector3 point = Vector3.Lerp(start, end, t);
            line.SetPosition(i, point);
        }
    }
}
