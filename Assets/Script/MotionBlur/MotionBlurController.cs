using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MotionBlurController : MonoBehaviour
{
    [SerializeField] UniversalRendererData rendererData;

    MotionBlurFeature motionBlurFeature;

    void Awake()
    {
        // Trouve le feature automatiquement
        foreach (var feature in rendererData.rendererFeatures)
        {
            if (feature is MotionBlurFeature mbf)
            {
                motionBlurFeature = mbf;
                break;
            }
        }
    }

    public void SetMotionBlur(bool enabled)
    {
        if (motionBlurFeature != null)
            motionBlurFeature.SetActive(enabled);
    }
}