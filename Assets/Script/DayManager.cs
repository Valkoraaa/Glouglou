using System.Collections;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    //[SerializeField] private int dayLight;
    public int totalThrow;
    public int actualThrow;
    public static DayManager Instance { get; private set; }
    [SerializeField] private DialogueData dialogue;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //StartOfDay(); ?
        //StartCoroutine(DayPassing());
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CountdownThrow()
    {
        if (actualThrow <= 0)
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
        //empeche le joueur de pecher et le sors de la zone
        EffectManager.Instance.ResetEffect();
        DialogueManager.Instance.StartDialogue(dialogue);
    }

    public void StartOfDay()
    {
        DayEffect();
        actualThrow = totalThrow;
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
}
