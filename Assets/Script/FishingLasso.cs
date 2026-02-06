using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingLasso : MonoBehaviour
{
    //for now lassos collider isnt trigger, can change if needed
    [SerializeField] private GameObject player;

    private void OnTriggerEnter(Collider collision)
    {

        if(collision.gameObject.tag == "water") //activate anyway when smth hit?
        {
            StartCoroutine(MissedThrow());
        }
        //qte ?
        else if(collision.gameObject.tag == "fish") { StartCoroutine(getFishToPlayer(collision.gameObject)); }
    }

    private IEnumerator getFishToPlayer(GameObject fish)
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

        Destroy(fish);
        ThrowLasso.Instance.hasLasso();
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

        //smoother way to get the lasso back in hand?
    }
}