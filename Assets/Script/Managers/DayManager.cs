using System.Collections;

using System.Collections.Generic;

using System.Linq;

using UnityEngine;



public class DayManager : MonoBehaviour

{

    //[SerializeField] private int dayLight;



    public static DayManager Instance { get; private set; }

    [SerializeField] private DialogueData dialogue;

    [SerializeField] private DialogueData lostDialogue;

    private int dayCount;

    public int numberOfFishToCatch;

    private int totalFishToCatch;

    public int numberOfFailsAllowed;

    public int fishCaught;

    public int numberOfFails;



    [SerializeField]

    private FishDatabaseSO fishDatabaseSO;



    [SerializeField] private GameObject[] badFishDisplays;

    public bool isNight;

    [SerializeField] private AudioSource windAudioSource;

    public Dictionary<int, string> badFishEffects = new Dictionary<int, string>();







    [SerializeField] private int numberOfBadFish;

    //public int totalThrow;

    //public int actualThrow;



    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()

    {

        totalFishToCatch = 6;

        numberOfFailsAllowed = 6;

        dayCount = 1;

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

        if ((numberOfFails >= numberOfFailsAllowed && !TutoManager.Instance.tuto) || fishCaught >= numberOfFishToCatch && !TutoManager.Instance.tuto && !isNight) //changer numberOfFish... en nombre de rat�

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



    public void EndOfDay()

    {

        Debug.Log("End Of Day");

        



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

        if (dayCount % 3 == 0 || dayCount == 1)

        {

            totalFishToCatch += 1;

        }

        PauseManager.Instance.canPause = true;

        Debug.Log("Start Of Day");

        DefineBadFish();

        DayEffect();

        //actualThrow = totalThrow;

        FishingLasso.Instance.fishNet.Clear();

        dayCount++;

        numberOfFails = 0;

        fishCaught = 0;

        numberOfFishToCatch = totalFishToCatch; // a regler

        

        //StartCoroutine(DayPassing());

        isNight = false;

        SpawnFish.Instance.ResetFishDatabase(); // 1. reset

        DefineBadFish();                        // 2. marque les prefabs

        SpawnFish.Instance.SpawningFish();      // 3. spawn + copie



        DayEffect();

        EffectManager.Instance.ApplyEffect();
        EffectManager.Instance.ResetEffect();

    }



    private void DayEffect()

    {

        if (Random.value <= 0.33f)

        {

            EffectManager.Instance.SetWind(true);

            Debug.Log("wind");

            windAudioSource.Play();

        }

        else { windAudioSource.Stop(); }

    }



    private void GameLost() //a completer //////////

    {

        //DialogueManager.Instance.StartDialogue(lostDialogue, false);

        GameOver.Instance.OnGameOver();

    }

    public void DefineBadFish()

    {

        badFishEffects.Clear();

        string[] possibleEffects = { "drunk", "drug", "sick", "sleep", "nostrength", "depression" };

        HashSet<int> alreadyChosen = new HashSet<int>();

        int i = 0;



        while (i < numberOfBadFish)

        {

            int chooseRarity = Random.Range(0, 4);

            List<Fish> rarityList = chooseRarity switch

            {

                0 => fishDatabaseSO.commonFish,

                1 => fishDatabaseSO.rareFish,

                2 => fishDatabaseSO.epicFish,

                _ => fishDatabaseSO.legendaryFish

            };



            Fish candidate = rarityList[Random.Range(0, rarityList.Count)];

            if (!alreadyChosen.Contains(candidate.data.id))

            {

                string chosenEffect = possibleEffects[Random.Range(0, possibleEffects.Length)];

                badFishEffects[candidate.data.id] = chosenEffect;

                alreadyChosen.Add(candidate.data.id);

                Debug.Log($"{candidate.data.species} → effet : {chosenEffect}");



                if (i < badFishDisplays.Length && badFishDisplays[i] != null)

                {

                    SpriteRenderer sr = badFishDisplays[i].GetComponent<SpriteRenderer>();

                    if (sr != null) sr.sprite = candidate.data.icon;

                }

                i++;

            }

        }

    }

}