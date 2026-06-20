using System.Collections;
using System.Numerics;
using UnityEngine;

public class TriggerOutCamping : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private DialogueData dialogueOutWhileNight;
    [SerializeField] private DialogueData dialogueNotEnoughFish;
    [SerializeField] private DialogueData dialogueGoToWork;
    [SerializeField] private DialogueData windDay;
    [SerializeField] private Transform tpPos;
    [SerializeField] private Transform otherTp;
    [SerializeField] private bool isCampTp;
    private bool hasToCheck = true;
    
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip musicBeach;
    [SerializeField] private AudioClip musicCamp;
    [SerializeField] private float duration;
    public AudioSource dayMusicAudioSource; 
    public static TriggerOutCamping Instance { get; private set; }

    void Start()
    {
        
    }
    private void Awake()
    {
        Instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Character.Instance.canMove) return;
        if (!other.CompareTag("Player")) return;

        /*if(isCampTp)
        {
            
        }
        else
        {
            
        }*/

        if (DayManager.Instance.isNight)
        {
            StartCoroutine(DialogueManager.Instance.WaitForEndOfDialogue(ChoseDialogue(), isCampTp ? tpPos.position : otherTp.position));
            ThrowLasso.Instance.canThrow = false;
            StartCoroutine(SwitchMusic(musicCamp));
           
        }
        else if ((DayManager.Instance.fishCaught < DayManager.Instance.numberOfFishToCatch) && hasToCheck)
        {
            StartCoroutine(DialogueManager.Instance.WaitForEndOfDialogue(ChoseDialogue(), isCampTp ? otherTp.position : tpPos.position));
            
            ThrowLasso.Instance.canThrow = true;
            StartCoroutine(SwitchMusic(musicBeach));
            
        }
        
        else if (!DayManager.Instance.isNight && DayManager.Instance.fishCaught >= DayManager.Instance.numberOfFishToCatch && isCampTp)
        {
            DayManager.Instance.isNight = true;
            ThrowLasso.Instance.canThrow = false;
            StartCoroutine(SwitchMusic(musicCamp));
            
            Debug.Log("2");
        }
        else { ThrowLasso.Instance.canThrow = true; }
    }

    private void OnTriggerExit(Collider other)
    {
        if(!other.CompareTag("Player")) return;
        if(isCampTp)
        {
            hasToCheck = true;
            Debug.Log("exit");
        }
    }

    private DialogueData ChoseDialogue()
    {
        if(DayManager.Instance.isNight) return dialogueOutWhileNight;
        else if (isCampTp && EffectManager.Instance.effects[0] == true)
        {
            hasToCheck = false;
            return windDay;
        }
        else if (isCampTp)
        {
            hasToCheck = false;
            return dialogueGoToWork;
        }
        else return dialogueNotEnoughFish;
    }

    private IEnumerator SwitchMusic(AudioClip audioClip)
    {
        if (musicSource.clip == audioClip)
        {
            yield break;
        }
        float startVolume = musicSource.volume;
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, time / duration);
            yield return null;
        }

        musicSource.volume = 0f;
        musicSource.clip = audioClip;
        musicSource.Play();

        time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0f, startVolume, time / duration);
            yield return null;
        }

        musicSource.volume = startVolume;
    }
    
}
