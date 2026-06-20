using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
//using Mono.Cecil.Cil;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Rendering;
using Unity.VisualScripting;

public class FishingLasso : MonoBehaviour
{
    //for now lassos collider isnt trigger, can change if needed
    [SerializeField] private GameObject player;
    [SerializeField] private DialogueData caughtDialogue;
    public int strenght;
    private SpriteRenderer visual;
    
    public bool hasToPlaySound;
    private Fish fish;
    [SerializeField] private float arcHeight;

    public static FishingLasso Instance;

    [Header("LassoImage")]
    [SerializeField] private Transform canvasLasso;
    [SerializeField] private Sprite lassoImage;

    [Header("Instantiation Poisson")]
    [SerializeField] private GameObject fishPrefab; // Le prefab du poisson à faire apparaître
    [SerializeField] private Transform spawnPoint;   // L'objet Empty qui sert de repère de position
    public List<GameObject> fishNet = new();


    private void Awake()
    {
        Instance = this;
        visual = GetComponent<SpriteRenderer>();
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

                Debug.Log("poid du poisson : " + finalWeight.ToString("F2"));
                Debug.Log("taille du poisson : " + finalSize.ToString("F2"));


                StartCoroutine(getFishToPlayer(fishScript));
            }
        }
        // else if (other.gameObject.CompareTag("tutoFish"))
        // {
        //     StartCoroutine(TutoManager.Instance.TutoFishing(other.gameObject));
        // }
        else if (other.gameObject.CompareTag("waterZone") && hasToPlaySound)
        {
            ThrowLasso.Instance.PlayRandomPlouf();
        }
    }

    private IEnumerator getFishToPlayer(Fish fish)
    {
        fish.gameObject.GetComponent<SphereCollider>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        hasToPlaySound = false;
        float duration = 1f;
        float elapsedTime = 0f;

        ThrowLasso.Instance.recallRope = true;

        if (ThrowLasso.Instance.GetComponentInChildren<Animator>() != null)
            ThrowLasso.Instance.GetComponentInChildren<Animator>().SetTrigger("TriggerRetour");

        Rope.Instance.endPoint = fish.GetComponent<Transform>();
        visual.enabled = false;

        canvasLasso.SetParent(fish.transform);
        canvasLasso.localPosition = Vector3.zero;
        canvasLasso.localRotation = Quaternion.identity;

        Vector3 fishStartPos = fish.transform.position;
        Vector3 thisStartPos = transform.position;
        Vector3 targetPos = player.transform.position;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            if (fish != null)
            {
                Vector3 pos = Vector3.Lerp(fishStartPos, targetPos, t);
                pos.y += arcHeight * 4f * t * (1f - t);
                fish.transform.position = pos;
            }
            transform.position = Vector3.Lerp(thisStartPos, targetPos, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasLasso.SetParent(null);
        canvasLasso.position = Vector3.zero;
        Rope.Instance.endPoint = Rope.Instance.originalEndPoint;

        if (!TutoManager.Instance.tuto)
        {
            // Stocke l'effet pour la prochaine journée
            if (DayManager.Instance.badFishEffects.TryGetValue(fish.data.id, out string effect))
            {
                if (!EffectManager.Instance.HasActiveEffect())
                    EffectManager.Instance.ChooseEffect(effect);
            }
            Shop.Instance.playerStackMoney += fish.data.price * Shop.Instance.moneyMultiplier;

            if (fishPrefab != null && spawnPoint != null)
            {
                GameObject nouveauPoisson = Instantiate(fishPrefab, spawnPoint.position, spawnPoint.rotation);
                nouveauPoisson.transform.SetParent(spawnPoint);
                nouveauPoisson.transform.localPosition = Vector3.zero;
                fishNet.Add(nouveauPoisson);
            }

            Destroy(fish.gameObject);
            ThrowLasso.Instance.hasLasso();
        }
        else
        {
            DialogueManager.Instance.StartDialogue(caughtDialogue, true);
        }

        visual.enabled = true;
        DayManager.Instance.fishCaught++;
        DayManager.Instance.CountdownThrow();
        ThrowLasso.Instance.lassoAudio.PlayOneShot(ThrowLasso.Instance.getLasso);
    }

    public void LaunchMissedThrow(bool water)
    {
        StartCoroutine(MissedThrow(water));
    }
    private IEnumerator MissedThrow(bool water)
    {
        hasToPlaySound = false;
        float duration = 1f;
        float elapsedTime = 0f;

        ThrowLasso.Instance.recallRope = true;

        if (ThrowLasso.Instance.GetComponentInChildren<Animator>() != null)
        {
            ThrowLasso.Instance.GetComponentInChildren<Animator>().SetTrigger("TriggerRetour");
        }

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
        ThrowLasso.Instance.lassoAudio.PlayOneShot(ThrowLasso.Instance.getLasso);
        //smoother way to get the lasso back in hand?
    }
}