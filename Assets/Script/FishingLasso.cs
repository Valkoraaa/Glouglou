using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingLasso : MonoBehaviour
{
    //for now lassos collider isnt trigger, can change if needed
    [SerializeField] private GameObject player;
    public int strenght;
    

    private Fish fish;

    public static FishingLasso Instance;

    private void Awake()
    {
        Instance = this;
    }


    private void OnCollisionEnter(Collision collision) //to check if you miss a fish
    {

        if(collision.gameObject.tag == "water") //activate anyway when smth hit?
        {
            StartCoroutine(MissedThrow());
        }
        //qte ?
    }

    private void OnTriggerEnter(Collider other) //to check if you hit a fish
    {
        //qte ?
        if (other.gameObject.tag == "fish" && strenght >= other.gameObject.GetComponent<Fish>().necessaryStrength && !ThrowLasso.Instance.recallRope) 
        {
            StartCoroutine(getFishToPlayer(other.gameObject.GetComponent<Fish>())); 
        }
    }

    private IEnumerator getFishToPlayer(Fish fish) //called when you hit a fish with lasso
    {
        float duration = 1f;
        float elapsedTime = 0f;

        ThrowLasso.Instance.recallRope = true;

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
        EffectManager.Instance.ChooseEffect(fish.TemporaryEffect);
        Shop.Instance.playerMoney += fish.price * Shop.Instance.moneyMultiplier;
        Destroy(fish.gameObject);
        ThrowLasso.Instance.hasLasso();
        //DayManager.Instance.actualThrow--;
        DayManager.Instance.fishCaught++;
        DayManager.Instance.CountdownThrow();
    }
    private IEnumerator MissedThrow()
    {
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
        DayManager.Instance.numberOfFails++;
        DayManager.Instance.CountdownThrow();

        //smoother way to get the lasso back in hand?
    }
}