using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] 
    private FishDatabaseSO fishDatabaseSO;

    [SerializeField]
    private GameObject BadFishOneDisplay;
    [SerializeField]
    private GameObject BadFishTwoDisplay;
    public bool isNight;
    [SerializeField] private AudioSource windAudioSource;



    [SerializeField] private int numberOfBadFish;
    //public int totalThrow;
    //public int actualThrow;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //StartOfDay(); ?
        //StartCoroutine(DayPassing());
        //actualThrow = totalThrow;

        //StartOfDay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        Instance = this;

    }
    public void CountdownThrow()
    {
        if (numberOfFails >= numberOfFailsAllowed && !TutoManager.Instance.tuto/*|| fishCaught >= numberOfFishToCatch || actualThrow <= 0 ?????*/) //changer numberOfFish... en nombre de rat�
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
        Debug.Log("End Of Day");
        EffectManager.Instance.ResetEffect();
        
        if (fishCaught < numberOfFishToCatch && !TutoManager.Instance.tuto)
        {
            GameLost();
        }
        else
        {
            DialogueManager.Instance.StartDialogue(dialogue, false);
            isNight = true;
        } //+ ouvrir la zone etc
        
    }

    public void StartOfDay()
    {
        PauseManager.Instance.canPause = true;
        numberOfFails = 0;
        fishCaught = 0;
        numberOfFishToCatch = 10;
        isNight = false;

        DayEffect();
        EffectManager.Instance.ApplyEffect();
        SpawnFish.Instance.ResetFishDatabase(); // 1. reset
        DefineBadFish();                        // 2. marque les prefabs
        SpawnFish.Instance.SpawningFish();      // 3. spawn + copie
    }

    private void DayEffect()
    {
        if (Random.value <= 0.33f)
        {
            EffectManager.Instance.effects[0] = true;
            Debug.Log("wind");
            windAudioSource.Play();
        }
        else {windAudioSource.Stop();}
    }

    private void GameLost() //a completer //////////
    {
        //DialogueManager.Instance.StartDialogue(lostDialogue, false);
        GameOver.Instance.OnGameOver();
    }

    private void DefineBadFish()
    {
        string[] possibleEffects = { "drunk", "exhaust", "sick", "depression" };

        for (int i = 0; i < numberOfBadFish; i++)
        {
            int chooseRarity = Random.Range(0, 4);
            List<Fish> rarityList = chooseRarity switch
            {
                0 => fishDatabaseSO.commonFish,
                1 => fishDatabaseSO.rareFish,
                2 => fishDatabaseSO.epicFish,
                _ => fishDatabaseSO.legendaryFish
            };
            ChangeStatus(rarityList, possibleEffects);
        }
    }

    private void ChangeStatus(List<Fish> rarityList, string[] possibleEffects)
    {
        int chooseFish = Random.Range(0, rarityList.Count);
        string chosenEffect = possibleEffects[Random.Range(0, possibleEffects.Length)];
        rarityList[chooseFish].IsBadForToday = true;
        rarityList[chooseFish].FishEffect = chosenEffect;
        Debug.Log($"{rarityList[chooseFish].data.species} → effet : {chosenEffect}");
    }
}
