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



    [SerializeField] private int numberOfBadFish;
    //public int totalThrow;
    //public int actualThrow;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //StartOfDay(); ?
        //StartCoroutine(DayPassing());
        Instance = this;
        //actualThrow = totalThrow;
        StartOfDay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CountdownThrow()
    {
        if (numberOfFails >= numberOfFailsAllowed /*|| fishCaught >= numberOfFishToCatch || actualThrow <= 0 ?????*/) //changer numberOfFish... en nombre de rat�
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
        
        if (fishCaught < numberOfFishToCatch)
        {
            GameOver();
        }
        else { DialogueManager.Instance.StartDialogue(dialogue); } //+ ouvrir la zone etc
        
    }

    public void StartOfDay()
    {
        Debug.Log("Start Of Day");
        DefineBadFish();
        DayEffect();
        //actualThrow = totalThrow;
        numberOfFails = 0;
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

    private void DefineBadFish()
    {
        int chooseRarity;

        for(int i = 0; i < numberOfBadFish; i++)
        {
            chooseRarity = Random.Range(0, 4);
            switch (chooseRarity)
            {
                case 0:
                    ChangeStatus(fishDatabaseSO.commonFish, i);
                    break;
                case 1:
                    ChangeStatus(fishDatabaseSO.rareFish, i);
                    break;
                case 2:
                    ChangeStatus(fishDatabaseSO.epicFish, i);
                    break;
                case 3:
                    ChangeStatus(fishDatabaseSO.legendaryFish, i);
                    break;
            }
            
        }
    }

    private void ChangeStatus(List<Fish> rarityList, int i)
    {
        int chooseFish;
        chooseFish = Random.Range(0, rarityList.Count);
        rarityList[chooseFish].IsBadForToday = true;
        Debug.Log(rarityList[chooseFish].Species);
        if (i % 2 == 0)
        {
            BadFishOneDisplay.GetComponent<MeshRenderer>().material = rarityList[chooseFish].baseMaterial;
        }
        else
        {
            BadFishTwoDisplay.GetComponent<MeshRenderer>().material = rarityList[chooseFish].baseMaterial;
        }

    }
}
