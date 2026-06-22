using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class UiGestion : MonoBehaviour
{
    public static UiGestion Instance;
    public TextMeshProUGUI fishCountText;
    public TextMeshProUGUI moneyText;
    [SerializeField] public Slider failSlider;
    public TextMeshProUGUI multText;

    public GameObject cross1;
    public GameObject cross2;
    public GameObject cross3;


    [Header("Ic�ne d�buff")]
    [SerializeField] private Image debuffIcon;
    [SerializeField] private Sprite drunkSprite;
    [SerializeField] private Sprite drugSprite;
    [SerializeField] private Sprite sickSprite;
    [SerializeField] private Sprite sleepSprite;
    [SerializeField] private Sprite noStrengthSprite;
    [SerializeField] private Sprite depressionSprite;

    [SerializeField] private GameObject firstCatchPanel;
    [SerializeField] private float displayDuration = 2.5f;

    private int lastFishCaught = -1;
    private int lastNumberOfFails = -1;

    void Start()
    {
        firstCatchPanel.SetActive(false);
        Instance = this;
        //UpdateFishCountText();
        InitFailSlider();
        debuffIcon.gameObject.SetActive(false);
        moneyText.text = "0";
        fishCountText.text = "0";
    }

    void Update()
    {
        moneyText.text = Shop.Instance.playerMoney.ToString();
        //if (DayManager.Instance.fishCaught != lastFishCaught)
        //{
        //    UpdateFishCountText();
        //}

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


    public void CheckStrenght(int strenght)
    {
        if(strenght == 1)
        {
            cross1.SetActive(false);
        }
        else if (strenght == 2)
        {
            cross2.SetActive(false);
        }
        else if (strenght == 3)
        {
            cross3.SetActive(false);
        }
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

    private void OnEnable()
    {
        FishingBookManager.OnFirstCatch += HandleFirstCatch;
    }

    private void OnDisable()
    {
        FishingBookManager.OnFirstCatch -= HandleFirstCatch;
    }

    private void HandleFirstCatch(int fishId)
    {
        StartCoroutine(ShowFirstCatchPanel());

    }

    private IEnumerator ShowFirstCatchPanel()
    {
        firstCatchPanel.SetActive(true);
        Image panelImage = firstCatchPanel.GetComponent<Image>();
        float elapsed = 0f;
        float duration = 2f;

        while (elapsed < duration)
        {
            float t = Mathf.PingPong(elapsed * 3f, 1f); // 3f = vitesse du pulse
            float scale = Mathf.Lerp(2f, 5f, t); // plus agressif
            firstCatchPanel.transform.localScale = Vector3.one * scale;
            elapsed += Time.deltaTime;
            yield return null;
        }

        firstCatchPanel.transform.localScale = Vector3.one;
        firstCatchPanel.SetActive(false);
    }
}