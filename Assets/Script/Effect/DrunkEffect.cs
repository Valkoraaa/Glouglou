using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DrunkEffect : MonoBehaviour
{
    [Header("Références")]
    public Volume volume;
    public Transform camera;

    [Header("Réglages généraux")]
    public float vitesse = 0.5f;
    public float intensiteGlobale = 1f;

    [Header("Lens Distortion")]
    public float lensDistortionMin = 0.9f;
    public float lensDistortionMax = 1f;

    [Header("Chromatic Aberration")]
    public float chromaticAberrationMin = 0.3f;
    public float chromaticAberrationMax = 1f;

    [Header("Vignette")]
    public float vignetteMin = 0.25f;
    public float vignetteMax = 0.55f;

    [Header("Tangage caméra")]
    public float rouletteAmplitude = 8f;
    public float tangageAmplitude = 4f;
    public float vitesseTangage = 0.7f;

    private LensDistortion lensDistortion;
    private ChromaticAberration chromaticAberration;
    private Vignette vignette;

    private Quaternion offsetPrecedent = Quaternion.identity;

    void Start()
    {
        if (volume == null)
            volume = GetComponent<Volume>();

        volume.profile.TryGet(out lensDistortion);
        volume.profile.TryGet(out chromaticAberration);
        volume.profile.TryGet(out vignette);
    }

    void Update()
    {

        float t = Time.time * vitesse;
        float noise = Mathf.PerlinNoise(t, 0f);
        float wave = (Mathf.Sin(t) * 0.5f + 0.5f);
        float blend = Mathf.Lerp(wave, noise, 0.5f) * intensiteGlobale;
        Debug.Log("blend = " + blend); // juste après le calcul de blend

        if (lensDistortion != null)
            lensDistortion.intensity.value = Mathf.Lerp(lensDistortionMin, lensDistortionMax, blend);

        if (chromaticAberration != null)
            chromaticAberration.intensity.value = Mathf.Lerp(chromaticAberrationMin, chromaticAberrationMax, blend);

        if (vignette != null)
            vignette.intensity.value = Mathf.Lerp(vignetteMin, vignetteMax, blend);
    }

    void LateUpdate()
    {
        if (camera == null) return;

        // On retire l'offset appliqué la frame précédente pour retrouver
        // la rotation "réelle" telle que définie par le FPS controller
        Quaternion baseRotation = camera.localRotation * Quaternion.Inverse(offsetPrecedent);

        float tt = Time.time * vitesseTangage;
        float roulis = (Mathf.PerlinNoise(tt, 10f) * 2f - 1f) * rouletteAmplitude * intensiteGlobale;
        float tangageVertical = (Mathf.PerlinNoise(tt, 50f) * 2f - 1f) * tangageAmplitude * intensiteGlobale;

        Quaternion nouvelOffset = Quaternion.Euler(tangageVertical, 0f, roulis);

        camera.localRotation = baseRotation * nouvelOffset;
        offsetPrecedent = nouvelOffset;
    }
}