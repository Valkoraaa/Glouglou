using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Volume))]
public class SleepyEffect : MonoBehaviour
{
    [Header("Vignette")]
    [SerializeField] private float minIntensity = 0.5f;
    [SerializeField] private float maxIntensity = 1f;

    [Header("Vitesse")]
    [SerializeField] private float minBlinkSpeed = 0.45f;
    [SerializeField] private float maxBlinkSpeed = 0.67f;
    [SerializeField] private float speedChangeSmoothness = 0.3f;

    private Vignette vignette;

    private float currentSpeed;
    private float targetSpeed;
    private float timer;

    private void Start()
    {
        Volume volume = GetComponent<Volume>();

        if (!volume.profile.TryGet(out vignette))
        {
            Debug.LogError("Aucune Vignette trouvée dans le Volume Profile.");
            return;
        }

        currentSpeed = Random.Range(minBlinkSpeed, maxBlinkSpeed);
        targetSpeed = currentSpeed;
    }

    private void Update()
    {
        if (vignette == null)
            return;

        // Choisit une nouvelle vitesse toutes les 2 à 5 secondes
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            targetSpeed = Random.Range(minBlinkSpeed, maxBlinkSpeed);
            timer = Random.Range(2f, 5f);
        }

        // Transition douce vers la nouvelle vitesse
        currentSpeed = Mathf.Lerp(
            currentSpeed,
            targetSpeed,
            speedChangeSmoothness * Time.deltaTime
        );

        float t = (Mathf.Sin(Time.time * currentSpeed) + 1f) * 0.5f;
        vignette.intensity.value = Mathf.Lerp(minIntensity, maxIntensity, t);
    }
}