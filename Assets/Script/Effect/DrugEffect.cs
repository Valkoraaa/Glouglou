using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class HueShiftLoop : MonoBehaviour
{
    public Volume volume;
    public float speed = 20f;
    private ColorAdjustments colorAdjustments;
    private float hue = 0f;

    void Start()
    {
        volume.profile = Instantiate(volume.profile);

        if (volume.profile.TryGet<ColorAdjustments>(out colorAdjustments) == false)
        {
            Debug.LogError("Color Adjustments not found in profile!");
        }
    }

    void Update()
    {
        if (colorAdjustments != null)
        {
            hue += Time.deltaTime * speed;
            if (hue > 180f) hue = -180f;
            colorAdjustments.hueShift.value = hue;
        }
    }
}