using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class UiGestion : MonoBehaviour
{
    public static UiGestion Instance;
    [SerializeField] private TextMeshProUGUI fishCountText;
    public TextMeshProUGUI moneyText;
    [SerializeField] private Slider failSlider;
    public TextMeshProUGUI multText;

    [Header("Ic�ne d�buff")]
    [SerializeField] private Image debuffIcon;
    [SerializeField] private Sprite drunkSprite;
    [SerializeField] private Sprite drugSprite;
    [SerializeField] private Sprite sickSprite;
    [SerializeField] private Sprite sleepSprite;
    [SerializeField] private Sprite noStrengthSprite;
    [SerializeField] private Sprite depressionSprite;

    private int lastFishCaught = -1;
    private int lastNumberOfFails = -1;

    void Start()
    {
        Instance = this;
        UpdateFishCountText();
        InitFailSlider();
        debuffIcon.gameObject.SetActive(false);
        moneyText.text = "0";
        fishCountText.text = "0";
    }

    void Update()
    {
        moneyText.text = Shop.Instance.playerMoney.ToString();
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

    public void UpdateDebuffIcon()
    {
        bool[] effects = EffectManager.Instance.effects;

        if (effects[0]) { debuffIcon.sprite = drunkSprite; debuffIcon.gameObject.SetActive(true); }
        else if (effects[1]) { debuffIcon.sprite = drugSprite; debuffIcon.gameObject.SetActive(true); }
        else if (effects[2]) { debuffIcon.sprite = sickSprite; debuffIcon.gameObject.SetActive(true); }
        else if (effects[3]) { debuffIcon.sprite = sleepSprite; debuffIcon.gameObject.SetActive(true); }
        else if (effects[4]) { debuffIcon.sprite = noStrengthSprite; debuffIcon.gameObject.SetActive(true); }
        else if (effects[5]) { debuffIcon.sprite = depressionSprite; debuffIcon.gameObject.SetActive(true); }
        else { debuffIcon.gameObject.SetActive(false); }
    }

    private void UpdateFailSlider()
    {
        lastNumberOfFails = DayManager.Instance.numberOfFails;
        failSlider.value = failSlider.maxValue - lastNumberOfFails;
    }
}