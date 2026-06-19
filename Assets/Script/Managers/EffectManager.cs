using System.Collections;

using UnityEngine;



public class EffectManager : MonoBehaviour

{

    [Header("Références effets")]

    [SerializeField] private GameObject drunkEffect;

    [SerializeField] private GameObject drugEffect;

    [SerializeField] private GameObject sickEffect;

    [SerializeField] private GameObject sleepEffect;

    [SerializeField] private GameObject depressionEffect;

    [SerializeField] private GameObject windZone;





    public bool[] effects;

    public static EffectManager Instance { get; private set; }



    public Vector3 windDirection = Vector3.right;

    public float windStrength = 4f;

    public bool isWindy;



    private int originalStrength;



    void Awake()

    {

        Instance = this;

        effects = new bool[] { false, false, false, false, false, false };

    }



    private void Start()

    {

        originalStrength = FishingLasso.Instance.strenght;

    }



    void FixedUpdate()

    {

        if (!isWindy) return;

        ThrowLasso.Instance.rb.AddForce(windDirection * windStrength, ForceMode.Force);

    }



    public bool HasActiveEffect()

    {

        for (int i = 0; i < effects.Length; i++)

            if (effects[i]) return true;

        return false;

    }

    public void ChooseEffect(string effect)

    {

        switch (effect)

        {

            case "drunk": effects[0] = true; Debug.Log("drunk"); break;

            case "drug": effects[1] = true; Debug.Log("drug"); break;

            case "sick": effects[2] = true; Debug.Log("sick"); break;

            case "sleep": effects[3] = true; Debug.Log("sleep"); break;

            case "nostrength": effects[4] = true; Debug.Log("nostrength"); break;

            case "depression": effects[5] = true; Debug.Log("depression"); break;

            case "none": Debug.Log("no effect"); break;

            default: Debug.Log("default"); break;

        }

    }



    public void ApplyEffect()

    {

        Drunk(effects[0]);

        Drug(effects[1]);

        Sick(effects[2]);

        Sleep(effects[3]);

        NoStrength(effects[4]);

        Depression(effects[5]);

    }



    public void ResetEffect()

    {

        for (int i = 0; i < effects.Length; i++)

            effects[i] = false;

        ApplyEffect();

    }



    public void SetWind(bool wantToActivate)

    {

        isWindy = wantToActivate;

        windZone.SetActive(wantToActivate);

    }



    private void Drunk(bool wantToActivate)

    {

        drunkEffect.SetActive(wantToActivate);

    }



    private void Drug(bool wantToActivate)

    {

        drugEffect.SetActive(wantToActivate);

    }



    private void Sick(bool wantToActivate)

    {

        sickEffect.SetActive(wantToActivate);

    }



    private void Sleep(bool wantToActivate)

    {

        sleepEffect.SetActive(wantToActivate);

    }



    private void Depression(bool wantToActivate)

    {

        depressionEffect.SetActive(wantToActivate);

    }



    private void NoStrength(bool wantToActivate)

    {

        if (wantToActivate) FishingLasso.Instance.strenght = 1;

        else FishingLasso.Instance.strenght = originalStrength;

    }

}