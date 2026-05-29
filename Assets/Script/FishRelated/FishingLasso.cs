using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Rendering;

public class FishingLasso : MonoBehaviour
{
    //for now lassos collider isnt trigger, can change if needed
    [SerializeField] private GameObject player;
    public int strenght;
    private MeshRenderer visual;
    [SerializeField] private Sprite lassoOnFish;
    
    public bool hasToPlaySound;
    private Fish fish;

    public static FishingLasso Instance;

    private void Awake()
    {
        Instance = this;
        visual = GetComponent<MeshRenderer>();
    }


    private void OnCollisionEnter(Collision collision) //to check if you miss a fish
    {
        
        if(collision.gameObject.tag == "water") //activate anyway when smth hit?
        {
            StartCoroutine(MissedThrow(true));
        }
        else
        {
            StartCoroutine(MissedThrow(false));
            //sound floor
        }
        //qte ?
    }

    private void OnTriggerEnter(Collider other) //to check if you hit a fish
    {
        Fish fishScript = other.gameObject.GetComponent<Fish>();

        


        if (other.gameObject.CompareTag("fish") && fishScript != null && fishScript.data != null)
        {
            int fishRarityValue = (int)fishScript.data.currentRarity;

            if (strenght >= fishRarityValue && !ThrowLasso.Instance.recallRope)
            {
                float fishWeight = other.gameObject.GetComponent<Fish>().Weight;
                float fishSize = other.gameObject.GetComponent<Fish>().Size;

                float finalWeight = fishScript.Weight * Random.Range(0.8f, 1.2f);
                float finalSize = fishScript.Size * Random.Range(0.8f, 1.2f);
                FishingBookManager.Instance.RegisterCatch(fishScript.data.id, finalWeight, finalSize);

                Debug.Log("poid du poisson : " + fishWeight.ToString("F2"));
                Debug.Log("taille du poisson : " + fishSize.ToString("F2"));


                StartCoroutine(getFishToPlayer(fishScript));
            }
        }
        else if (other.gameObject.CompareTag("tutoFish"))
        {
            StartCoroutine(TutoManager.Instance.TutoFishing(other.gameObject));
        }
        else if (other.gameObject.CompareTag("waterZone") && hasToPlaySound)
        {
            ThrowLasso.Instance.PlayRandomPlouf();
        }
    }

    private IEnumerator getFishToPlayer(Fish fish)
    {
        hasToPlaySound = false;
        float duration = 1f;
        float elapsedTime = 0f;

        ThrowLasso.Instance.recallRope = true;
        Rope.Instance.endPoint = fish.GetComponent<Transform>();
        visual.enabled = false;

        ////
        GameObject extraSprite = new GameObject("ExtraSprite");

        extraSprite.transform.SetParent(fish.transform);

        extraSprite.transform.localPosition = Vector3.zero;

        SpriteRenderer sr = extraSprite.AddComponent<SpriteRenderer>();
        sr.sprite = lassoOnFish;
        ////

        Vector3 fishStartPos = fish.transform.position;
        Vector3 thisStartPos = transform.position;
        Vector3 targetPos = player.transform.position;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            if(fish != null) fish.transform.position = Vector3.Lerp(fishStartPos, targetPos, t);
            transform.position = Vector3.Lerp(thisStartPos, targetPos, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // raccrocher le lasso au joueur et lui donner le poissson + qte? ;; suite du code temporaire
        //animation?
        Rope.Instance.endPoint = Rope.Instance.originalEndPoint;
        EffectManager.Instance.ChooseEffect(fish.TemporaryEffect);
        Shop.Instance.playerMoney += fish.data.price * Shop.Instance.moneyMultiplier;
        Destroy(fish.gameObject);
        ThrowLasso.Instance.hasLasso();
        visual.enabled = true;
        //DayManager.Instance.actualThrow--;
        DayManager.Instance.fishCaught++;
        DayManager.Instance.CountdownThrow();

    }
    private IEnumerator MissedThrow(bool water)
    {
        hasToPlaySound = false;
        float duration = 1f;
        float elapsedTime = 0f;

        ThrowLasso.Instance.recallRope = true;

        Vector3 thisStartPos = transform.position;
        Vector3 targetPos = player.transform.position;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            transform.position = Vector3.Lerp(thisStartPos, targetPos, t);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        ThrowLasso.Instance.hasLasso();

        //DayManager.Instance.actualThrow --;
        if (water)
        {
            DayManager.Instance.numberOfFails++;
            DayManager.Instance.CountdownThrow();
        }
        
        //smoother way to get the lasso back in hand?
    }
}