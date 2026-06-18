using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutoManager : MonoBehaviour
{
    public static TutoManager Instance;
    public bool tuto;
    public int dialogueCounter;
    public bool endOfDialogue;
    public bool fadeFinished;
    public bool hasToBack;
    public bool blockActive;
    [SerializeField] private GameObject tutoFish;
    [SerializeField] private GameObject tutoBlock;
    [SerializeField] private GameObject player;
    
    [Header("Dialogues")]
    [SerializeField] private DialogueData tutoDialogue;
    [SerializeField] private DialogueData tutoDialogue2;
    //[SerializeField] private DialogueData caughtDialogue;
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
            StartCoroutine(tutoEnumerator());
            
        }
        else
        {
            StartCoroutine(StartTheDay());
            Character.Instance.canMove = true;
            Character.Instance.canMoveCam = true;
            Character.Instance.stopChara = false;
            Character.Instance.transform.position = new Vector3(1478.14f, 180.59f, 1102.03f);
        }
    }

    private IEnumerator tutoEnumerator()
    {
        yield return new WaitUntil(() => DialogueManager.Instance != null && DayManager.Instance != null && !SceneManager.GetSceneByName("Opening").isLoaded);
        

        DialogueManager.Instance.StartDialogue(tutoDialogue, true);
        tutoFish.SetActive(true);
        tutoBlock.SetActive(true);
    }

    void Update()
    {
        if(blockActive)
        {
            blockActive = false;
            tutoBlock.SetActive(true);
        }
        if(endOfDialogue)
        {
            endOfDialogue = false;
            if(dialogueCounter == 1) { EndOfFirstDialogue();}
            else if (dialogueCounter == 2 && hasToBack)
            {
                hasToBack = false;
            }
            else if (dialogueCounter == 2)
            {
                Character.Instance.canMove = true;
                Character.Instance.canMoveCam = true;
            }
            else if (dialogueCounter == 3)
            {
                StartCoroutine(WaitABit());
            }
        }
    }

    public void EndOfFirstDialogue()
    {
        StartCoroutine(ShowCamping());
    }

    // public IEnumerator TutoFishing(GameObject fish)
    // {
    //     float duration = 1f;
    //     float elapsedTime = 0f;

    //     ThrowLasso.Instance.recallRope = true;

    //     Vector3 fishStartPos = fish.transform.position;
    //     Vector3 thisStartPos = transform.position;
    //     Vector3 targetPos = Character.Instance.GetComponent<Transform>().transform.position;
    //     Transform lassoTransform = FishingLasso.Instance.GetComponent<Transform>();
    //     while (elapsedTime < duration)
    //     {
    //         float t = elapsedTime / duration;

    //         if (fish != null)
    //         {
    //             Vector3 pos = Vector3.Lerp(fishStartPos, targetPos, t);


    //             // Parabole : 0 → 1 → 0
    //             pos.y += 7 * 4f * t * (1f - t);

    //             fish.transform.position = pos;
    //         }

    //         lassoTransform.position = Vector3.Lerp(thisStartPos, targetPos, t);

    //         elapsedTime += Time.deltaTime;
    //         yield return null;
    //     }
    //     //move player vers pnj
    //     DialogueManager.Instance.StartDialogue(caughtDialogue, true);
    //     Character.Instance.canMove = false;
    //     Character.Instance.canMoveCam = false;
    // }

    private IEnumerator ShowCamping()
    {
        playerCamera.transform.GetChild(0).SetParent(null);
        Character.Instance.cinematic = true;

        startPos = playerCamera.transform.localPosition;
        startRot = playerCamera.transform.localRotation;
        Debug.Log(startPos);
        Vector3 targetPos = new Vector3(17.38f, 65.36f, 74.84f);
        Quaternion targetRot = Quaternion.Euler(2.455f, 17.205f, -0.108f);

        // Aller
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            float lerp = t / duration;

            playerCamera.transform.localPosition = Vector3.Lerp(startPos, targetPos, lerp);
            playerCamera.transform.localRotation = Quaternion.Slerp(startRot, targetRot, lerp);

            yield return null;
        }

        yield return new WaitForSeconds(1.5f);
        // Retour
        t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            float lerp = t / duration;

            playerCamera.transform.localPosition = Vector3.Lerp(targetPos, startPos, lerp);
            playerCamera.transform.localRotation = Quaternion.Slerp(targetRot, startRot, lerp);

            yield return null;
        }
        playerCamera.transform.localPosition = startPos;
        playerCamera.transform.localRotation = startRot;
        Character.Instance.cinematic = false;
        ThrowLasso.Instance.hasLasso();
        DialogueManager.Instance.StartDialogue(tutoDialogue2, true);
    }

    private IEnumerator WaitABit()
    {
        tutoBlock.SetActive(false);
        Character.Instance.canMoveCam = false;
        Character.Instance.stopChara = true;
        fadeFinished = false;
        UiFadeManager.Instance.FadeTp(new Vector3(1478.14f, 180.59f, 1102.03f)); //mouvement de cam?
        yield return new WaitForSeconds(0.5f);
        player.transform.rotation = Quaternion.Euler(0, -180, 0);
        Character.Instance.xRotation = 0;
        playerCamera.transform.localRotation = Quaternion.Euler(0, 0, 0);
        ThrowLasso.Instance.hasLasso();
        Character.Instance.stopChara = true;
        Character.Instance.canMove = false;
        Character.Instance.canMoveCam = false;
        yield return new WaitUntil(() => fadeFinished);


        DayManager.Instance.isNight = true;
        
        
       
                
        DialogueManager.Instance.StartDialogue(tutoDialogue3, true);
    }

    private IEnumerator StartTheDay()
    {
        yield return new WaitForSeconds(0.1f);
        DayManager.Instance.StartOfDay();
    }
}
