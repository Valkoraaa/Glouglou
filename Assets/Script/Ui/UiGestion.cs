using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiGestion : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fishCountText;
    [SerializeField] private Slider failSlider;

    private int lastFishCaught = -1;
    private int lastNumberOfFails = -1;

    void Start()
    {
        UpdateFishCountText();
        InitFailSlider();
    }

    void Update()
    {
        if (DayManager.Instance.fishCaught != lastFishCaught)
        {
            UpdateFishCountText();
        }

        if (DayManager.Instance.numberOfFails != lastNumberOfFails)
        {
            UpdateFailSlider();
        }
    }

    private void UpdateFishCountText()
    {
        lastFishCaught = DayManager.Instance.fishCaught;
        fishCountText.text = lastFishCaught.ToString();
    }

    private void InitFailSlider()
    {
        failSlider.minValue = 0;
        failSlider.maxValue = DayManager.Instance.numberOfFailsAllowed;
        UpdateFailSlider();
    }

    private void UpdateFailSlider()
    {
        lastNumberOfFails = DayManager.Instance.numberOfFails;
        failSlider.value = failSlider.maxValue - lastNumberOfFails;
    }
}