using System.Collections;
using UnityEngine;

public class TutoManager : MonoBehaviour
{
    public static TutoManager Instance;
    public bool tuto;
    public int dialogueCounter;
    public bool endOfDialogue = false;
    
    [Header("Dialogues")]
    [SerializeField] private DialogueData tutoDialogue;
    [SerializeField] private DialogueData tutoDialogue2;
    [SerializeField] private DialogueData caughtDialogue;
    [SerializeField] private DialogueData tutoDialogue3;

    [Header("Camera")]
    [SerializeField] private GameObject playerCamera;
    public Vector3 offset = new Vector3(0, 0, 5);
    public float duration = 1f;
    Vector3 startPos;
    Quaternion startRot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        if(tuto)
        {
            DialogueManager.Instance.StartDialogue(tutoDialogue);
        }
    }

    void Update()
    {
        if(endOfDialogue)
        {
            endOfDialogue = false;
            if(dialogueCounter == 1) { EndOfFirstDialogue(); Debug.Log("1");}
            else if (dialogueCounter == 2)
            {
                Character.Instance.canMove = true;
                Character.Instance.canMoveCam = true;
                Debug.Log("2");
            }
            else if (dialogueCounter == 3)
            {
                UiFadeManager.Instance.FadeTp(new Vector3(1478.14f, 180.59f, 1102.03f)); //mouvement de cam?
                DayManager.Instance.isNight = true;
                tuto = false;
                DialogueManager.Instance.StartDialogue(tutoDialogue3);
            }
        }
    }

    public void EndOfFirstDialogue()
    {
        StartCoroutine(ShowCamping());
    }

    public IEnumerator TutoFishing(GameObject fish)
    {
        float duration = 1f;
        float elapsedTime = 0f;

        ThrowLasso.Instance.recallRope = true;

        Vector3 fishStartPos = fish.transform.position;
        Vector3 thisStartPos = transform.position;
        Vector3 targetPos = Character.Instance.GetComponent<Transform>().transform.position;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            if(fish != null) fish.transform.position = Vector3.Lerp(fishStartPos, targetPos, t);
            transform.position = Vector3.Lerp(thisStartPos, targetPos, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        //move player vers pnj
        DialogueManager.Instance.StartDialogue(caughtDialogue);
        Character.Instance.canMove = false;
        Character.Instance.canMoveCam = false;
    }

    private IEnumerator ShowCamping()
    {
        startPos = transform.position;
        startRot = transform.rotation;

        Vector3 targetPos = startPos + transform.forward * offset.z;
        Quaternion targetRot = Quaternion.Euler(startRot.eulerAngles + new Vector3(0, 30, 0));

        // Aller
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            float lerp = t / duration;

            transform.position = Vector3.Lerp(startPos, targetPos, lerp);
            transform.rotation = Quaternion.Slerp(startRot, targetRot, lerp);

            yield return null;
        }

        // Retour
        t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            float lerp = t / duration;

            transform.position = Vector3.Lerp(targetPos, startPos, lerp);
            transform.rotation = Quaternion.Slerp(targetRot, startRot, lerp);

            yield return null;
        }
        DialogueManager.Instance.StartDialogue(tutoDialogue2);
    }
}
