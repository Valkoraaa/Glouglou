using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class HueShiftLoop : MonoBehaviour
{
    public Volume volume;
    public float speed = 20f;

    private ColorAdjustments colorAdjustments;

    void Start()
    {
        if (volume.profile.TryGet<ColorAdjustments>(out colorAdjustments) == false)
        {
            Debug.LogError("Color Adjustments not found in profile!");
        }
    }

    void Update()
    {
        if (colorAdjustments != null)
        {
            // Mathf.PingPong fait un aller-retour entre 0 et 100 selon le temps
            colorAdjustments.hueShift.value = Mathf.PingPong(Time.time * speed, 100f);
        }
    }
}