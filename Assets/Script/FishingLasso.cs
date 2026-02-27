using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingLasso : MonoBehaviour
{
    //for now lassos collider isnt trigger, can change if needed
    [SerializeField] private GameObject player;

    private void OnCollisionEnter(Collision collision)
    {

        if(collision.gameObject.tag == "water") //activate anyway when smth hit?
        {
            StartCoroutine(MissedThrow());
        }
        //qte ?
    }

    private void OnTriggerEnter(Collider other)
    {
        //qte ?
        if (other.gameObject.tag == "fish") { StartCoroutine(getFishToPlayer(other.gameObject.GetComponent<Fish>())); }
    }

    private IEnumerator getFishToPlayer(Fish fish)
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
        Destroy(fish.gameObject);
        ThrowLasso.Instance.hasLasso();
        DayManager.Instance.actualThrow -= 1;
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

        DayManager.Instance.actualThrow -= 1;
        DayManager.Instance.CountdownThrow();

        //smoother way to get the lasso back in hand?
    }
}