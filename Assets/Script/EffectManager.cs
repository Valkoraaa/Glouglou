using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EffectManager : MonoBehaviour
{
    [Header("R�f�rences")]
    [SerializeField] private RectTransform upExhaust;
    [SerializeField] private RectTransform downExhaust;
    [SerializeField] private GameObject player;
    private float originalThrowForce;
    private bool[] effects = { false, false, false, false };
    public static EffectManager Instance { get; private set; }
    public Vector3 windDirection = Vector3.right;
    public float windStrength = 2f;

    [Header("Tests")]
    public bool activateExhaust;
    public bool desactivateExhaust;
    public bool activateWind;
    public float exhaustRange = 0.34f;
    public bool isWindy;
    [Header("TestsCam")]
    public Volume volume;

    LensDistortion lens;
    ChromaticAberration chroma;



    void Awake()
    {
        Instance = this;
        originalThrowForce = ThrowLasso.Instance.force;

        volume.profile.TryGet(out lens);
        volume.profile.TryGet(out chroma);
    }

    public void Update() //temp
    {
        if (activateExhaust) { Exhaust(true); }
        if (desactivateExhaust) { Exhaust(false); }
        if (activateWind) { Wind(true); }

        float t = Time.time;

        lens.intensity.value = Mathf.Sin(t * 0.7f) * 10f;
        chroma.intensity.value = Mathf.Abs(Mathf.Sin(t * 0.9f)) * 1f;
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
        ApplyEffect();
    }
    public void ChooseEffect(string effect)
    {
        switch (effect)
        {
            case "wind":
                if (Random.value <= 0.2f)
                {
                    effects[0] = true;
                }
                break;
            case "drunk":
                if (Random.value <= 0.2f)
                {
                    effects[1] = true;
                }
                break;
            case "exhaust":
                if (Random.value <= 0.2f)
                {
                    effects[2] = true;
                }
                break;
            case "sick":
                if (Random.value <= 0.2f)
                {
                    effects[3] = true;
                }
                break;
        }   
    }

    private void ApplyEffect()
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
    }

    private void Wind(bool wantToActivate)
    {
        isWindy = wantToActivate;
        activateWind = false;
    }

    private void Drunk (bool wantToActivate)
    {
        //mettre un effet a la cam
    }

    public void Exhaust (bool wantToActivate)
    {
        StartCoroutine(EyesClosing(wantToActivate));
        activateExhaust = false;
        desactivateExhaust = false;
    }

    private void Sick (bool wantToActivate)
    {
        if (wantToActivate) { ThrowLasso.Instance.force = ThrowLasso.Instance.force * 0.66f; }
        else { ThrowLasso.Instance.force = originalThrowForce; }
        //l�ger canvas vert?
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
        //rajouter un canvas noir avec l opacit� qui varie?
    }
}
