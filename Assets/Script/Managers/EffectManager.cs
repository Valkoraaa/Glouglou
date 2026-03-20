using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EffectManager : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private RectTransform upExhaust;
    [SerializeField] private RectTransform downExhaust;

    [SerializeField] private ParticleSystem rain;
    [SerializeField] private Volume depressionEffect;


    [SerializeField] private GameObject player;
    private float originalThrowForce;
    public bool[] effects = { false, false, false, false, false }; //more false if more effects
    public static EffectManager Instance { get; private set; }
    public Vector3 windDirection = Vector3.right;
    public float windStrength = 2f;
    public Coroutine exhaustEnumerator;

    [Header("Tests")]
    public bool activateExhaust;
    public bool desactivateExhaust;
    public bool activateWind;
    public bool activateDrunk;
    public float exhaustRange = 0.34f;
    public bool isWindy;
    public float lensStrengh;
    public float chromStrengh;
    private bool checkForDrunk;
    [Header("TestsCam")]
    public Volume volume;

    LensDistortion lens;
    ChromaticAberration chroma;



    void Awake()
    {
        Instance = this;
        originalThrowForce = ThrowLasso.Instance.force;

        rain.gameObject.SetActive(false);
        depressionEffect.gameObject.SetActive(false);
        volume.profile.TryGet(out lens); //drunk effect test
        volume.profile.TryGet(out chroma);
    }

    public void Update() //temp
    {
        if (activateExhaust) { Exhaust(true); }
        if (desactivateExhaust) { Exhaust(false); }
        if (activateWind) { Wind(true); }
        if (activateDrunk) { Drunk(true); }


        if (checkForDrunk) { DrunkEffect(); }
    }

    void FixedUpdate()
    {
        if (!isWindy) return;
        ThrowLasso.Instance.rb.AddForce(EffectManager.Instance.windDirection * EffectManager.Instance.windStrength, ForceMode.Force);
    }
    
    public void ResetEffect()
    {
        for (int i = 0; i < effects.Length; i++)
        {
            effects[i] = false;
        }
        if(exhaustEnumerator != null)
            StopCoroutine(exhaustEnumerator);
        ApplyEffect();
    }
    public void ChooseEffect(string effect)
    {
        switch (effect)
        {
            case "drunk":
                effects[1] = true;
                Debug.Log("drunk");
                break;
            case "exhaust":
                effects[2] = true;
                Debug.Log("exhaust");
                break;
            case "sick":
                effects[3] = true;
                Debug.Log("sick");
                break;
            case "depression":
                effects[4] = true;
                break;
            case "none":
                Debug.Log("no effect");
                break;
        }
    }

    public void ApplyEffect()
    {
        /*for (int i = 0; i < effects.Length; i++)
        {
            if (effects[i])
            {
                Debug.Log("Effect " + i + " is active");
                //appeler un void qui active l effet
            }
        }*/
        Wind(effects[0]);
        Drunk(effects[1]);
        Exhaust(effects[2]);
        Sick(effects[3]);
        Depression(effects[4]);
    }

    private void Wind(bool wantToActivate)
    {
        isWindy = wantToActivate;
        activateWind = false;
    }

    private void Drunk (bool wantToActivate)
    {
        if(wantToActivate) { checkForDrunk = true; }
        else { checkForDrunk = false; }
        //a ameliorer
    }

    public void Exhaust (bool wantToActivate)
    {
        if (upExhaust == null)
        {
            return;
        }
        else
        {
            exhaustEnumerator = StartCoroutine(EyesClosing(wantToActivate));
        }
        activateExhaust = false;
        desactivateExhaust = false;
        //tous les poissons sont les memes?
    }

    private void Sick (bool wantToActivate)
    {
        if (wantToActivate) { ThrowLasso.Instance.force = ThrowLasso.Instance.force * 0.66f; }
        else { ThrowLasso.Instance.force = originalThrowForce; }
        //leger canvas vert?
    }


    private void Depression(bool wantToActivate)
    {
        rain.gameObject.SetActive(true);
        depressionEffect.gameObject.SetActive(true);
    }


    private IEnumerator EyesClosing(bool opening)
    {

        float duration = 1f;
        float elapsed = 0f;
        float actualRange = exhaustRange;

        Vector2 startPosUp = upExhaust.anchoredPosition;
        Vector2 startPosDown = downExhaust.anchoredPosition;
        if (!opening) { actualRange = exhaustRange * 3; }
        float targetYUp = -Screen.height * actualRange;
        float targetYDown = Screen.height * actualRange;
        Vector2 targetPosUp = new Vector2(startPosUp.x, targetYUp);
        Vector2 targetPosDown = new Vector2(startPosDown.x, targetYDown);

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            upExhaust.anchoredPosition = Vector2.Lerp(startPosUp, targetPosUp, t);
            downExhaust.anchoredPosition = Vector2.Lerp(startPosDown, targetPosDown, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        upExhaust.anchoredPosition = targetPosUp;
        downExhaust.anchoredPosition = targetPosDown;
        //rajouter un canvas noir avec l opacite qui varie?
        yield return new WaitForSeconds(2f);
        StartCoroutine(EyesClosing(false));
        while (true)
        {
            yield return new WaitForSeconds(Random.value * 20);
            StartCoroutine(EyesClosing(true));
        }
    }

    private void DrunkEffect() //a ameliorer
    {
        float t = Time.time;
        lens.intensity.value = Mathf.Sin(t * 0.7f) * lensStrengh;
        chroma.intensity.value = Mathf.Abs(Mathf.Sin(t * 0.9f)) * chromStrengh;
    }
}
