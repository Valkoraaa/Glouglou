using System.Collections;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    //[SerializeField] private int dayLight;
    
    public static DayManager Instance { get; private set; }
    [SerializeField] private DialogueData dialogue;
    [SerializeField] private DialogueData lostDialogue;
    public int numberOfFishToCatch;
    public int numberOfFailsAllowed;
    public int fishCaught;
    public int numberOfFails;
    //public int totalThrow;
    //public int actualThrow;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //StartOfDay(); ?
        //StartCoroutine(DayPassing());
        Instance = this;
        //actualThrow = totalThrow;
        numberOfFails = numberOfFailsAllowed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CountdownThrow()
    {
        if (numberOfFails >= numberOfFailsAllowed /*|| fishCaught >= numberOfFishToCatch || actualThrow <= 0 ?????*/) //changer numberOfFish... en nombre de raté
        {
            EndOfDay();
        }
    }

    //IEnumerator DayPassing()
    //{
    //    yield return new WaitForSeconds(dayLight);
    //    EndOfDay();
    //    Debug.Log("endOfDay");


    //    //temp
    //    yield return new WaitForSeconds(120f);
    //    StartOfDay();
    //    Debug.Log("startOfDay");
    //}

    private void EndOfDay()
    {
        EffectManager.Instance.ResetEffect();
        
        if (fishCaught < numberOfFishToCatch)
        {
            GameOver();
        }
        else { DialogueManager.Instance.StartDialogue(dialogue); } //+ ouvrir la zone etc
        
    }

    public void StartOfDay()
    {
        DayEffect();
        //actualThrow = totalThrow;
        numberOfFails = numberOfFailsAllowed;
        fishCaught = 0;
        numberOfFishToCatch = 10; // a regler
        EffectManager.Instance.ApplyEffect();
        //StartCoroutine(DayPassing());
    }

    private void DayEffect()
    {
        if (Random.value <= 0.2f)
        {
            EffectManager.Instance.effects[0] = true;
            Debug.Log("wind");
        }
    }

    private void GameOver() //a completer //////////
    {
        DialogueManager.Instance.StartDialogue(lostDialogue);
    }
}
