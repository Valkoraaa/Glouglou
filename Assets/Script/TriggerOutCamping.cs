using System.Collections;
using System.Numerics;
using UnityEngine;

public class TriggerOutCamping : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private DialogueData dialogueOutWhileNight;
    [SerializeField] private DialogueData dialogueNotEnoughFish;
    [SerializeField] private DialogueData dialogueGoToWork;
    [SerializeField] private Transform tpPos;
    [SerializeField] private Transform otherTp;
    [SerializeField] private bool isCampTp;
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("triggerEnter");
        if (DayManager.Instance.isNight || DayManager.Instance.fishCaught < DayManager.Instance.numberOfFishToCatch)
        {
            StartCoroutine(WaitForEndOfDialogue(ChoseDialogue(), isCampTp ? otherTp.position : tpPos.position));
        }
        // else if (DayManager.Instance.fishCaught < DayManager.Instance.numberOfFishToCatch)
        // {
        //     StartCoroutine(WaitForEndOfDialogue(dialogueNotEnoughFish, new UnityEngine.Vector3(transform.position.x+2, transform.position.y, transform.position.z-2)));
        // }
    }

    private DialogueData ChoseDialogue()
    {
        if(DayManager.Instance.isNight) return dialogueOutWhileNight;
        else if (isCampTp) return dialogueGoToWork;
        else return dialogueNotEnoughFish;
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
