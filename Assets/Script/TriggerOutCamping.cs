using System.Collections;
using System.Numerics;
using UnityEngine;

public class TriggerOutCamping : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private DialogueData dialogueOutWhileNight;
    [SerializeField] private DialogueData dialogueNotEnoughFish;
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("triggerEnter");
        if (DayManager.Instance.isNight)
        {
            StartCoroutine(WaitForEndOfDialogue(dialogueOutWhileNight, new UnityEngine.Vector3(1482.33f, 180.97f, 1105.83f)));
        }
        else if (DayManager.Instance.fishCaught < DayManager.Instance.numberOfFishToCatch)
        {
            StartCoroutine(WaitForEndOfDialogue(dialogueNotEnoughFish, new UnityEngine.Vector3(1482.33f, 180.97f, 1105.83f)));
        }
    }

    private IEnumerator WaitForEndOfDialogue(DialogueData dialogue, UnityEngine.Vector3 tpPos)
    {
        Debug.Log("Enumerator");
        DialogueManager.Instance.StartDialogue(dialogue);
        yield return new WaitUntil(() => !DialogueManager.Instance.isInDialogue);
        Debug.Log("tp");
        UiFadeManager.Instance.FadeTp(tpPos);
    }
}
