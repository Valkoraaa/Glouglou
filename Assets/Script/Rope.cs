using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Rope : MonoBehaviour
{
    public static Rope Instance { get; private set; }
    [SerializeField] private Transform startPoint;
    public Transform endPoint;
    public Transform originalEndPoint;
    

    [Header("Rope Settings")]
    [SerializeField] private int segments = 25;
    [SerializeField] private float waveAmplitude = 0.2f;
    [SerializeField] private float waveFrequency = 2f;
    [SerializeField] private float waveSpeed = 5f;

    [SerializeField] private float alignSpeed = 8f;
    [SerializeField] private float alignThreshold = 0.01f;
    private bool canRecall;

    private LineRenderer line;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = segments;
        Instance = this;
        originalEndPoint = endPoint;
    }

    void FixedUpdate()
    {
        if(ThrowLasso.Instance.hasThrown && !ThrowLasso.Instance.recallRope){ DrawRope(); canRecall = false; }
        else if (ThrowLasso.Instance.hasThrown && ThrowLasso.Instance.recallRope && !canRecall) { AlignRope(); }
        else if (ThrowLasso.Instance.hasThrown && ThrowLasso.Instance.recallRope && canRecall) { RecallRope(); }
        else { line.enabled = false; }
    }

    void DrawRope()
    {
        line.enabled = true;
        
        Vector3 start = startPoint.position;
        Vector3 end = endPoint.position;

        Vector3 dir = (end - start).normalized;

        Vector3 side = Vector3.Cross(Vector3.up, dir).normalized;
        Vector3 up = Vector3.Cross(dir, side).normalized;

        for (int i = 0; i < segments; i++)
        {
            float t = i / (float)(segments - 1);

            Vector3 point = Vector3.Lerp(start, end, t);

            float wave = Mathf.Sin(t * waveFrequency + Time.time * waveSpeed)
                       * waveAmplitude
                       * Mathf.Sin(t * Mathf.PI);

            // direction aleatoire
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

    void AlignRope()
    {
        Vector3 start = startPoint.position;
        Vector3 end = endPoint.position;

        bool isStraightEnough = true;

        for (int i = 0; i < segments; i++)
        {
            float t = i / (float)(segments - 1);

            Vector3 current = line.GetPosition(i);
            Vector3 target = Vector3.Lerp(start, end, t);

            Vector3 aligned = Vector3.Lerp(current, target, Time.deltaTime * alignSpeed);
            line.SetPosition(i, aligned);

            if (Vector3.Distance(aligned, target) > alignThreshold)
                isStraightEnough = false;
        }

        if (isStraightEnough)
            canRecall = true;
    }
}
