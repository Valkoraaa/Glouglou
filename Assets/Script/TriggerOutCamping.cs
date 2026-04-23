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
    private bool hasToCheck = true;
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player")) return;
        if (DayManager.Instance.isNight)
        {
            StartCoroutine(WaitForEndOfDialogue(ChoseDialogue(), isCampTp ? tpPos.position : otherTp.position));
            ThrowLasso.Instance.canThrow = false;
        }
        else if ((DayManager.Instance.fishCaught < DayManager.Instance.numberOfFishToCatch) && hasToCheck)
        {
            StartCoroutine(WaitForEndOfDialogue(ChoseDialogue(), isCampTp ? otherTp.position : tpPos.position));
            ThrowLasso.Instance.canThrow = true;
        }
        
        else if (!DayManager.Instance.isNight && DayManager.Instance.fishCaught >= DayManager.Instance.numberOfFishToCatch && isCampTp)
        {
            DayManager.Instance.isNight = true;
            ThrowLasso.Instance.canThrow = false;
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
        else if (isCampTp)
        {
            hasToCheck = false;
            return dialogueGoToWork;
        }
        else return dialogueNotEnoughFish;
    }

    private IEnumerator WaitForEndOfDialogue(DialogueData dialogue, UnityEngine.Vector3 tpPos)
    {
        DialogueManager.Instance.StartDialogue(dialogue);
        yield return new WaitUntil(() => !DialogueManager.Instance.isInDialogue);
        Debug.Log("tp");
        UiFadeManager.Instance.FadeTp(tpPos);
    }
}
