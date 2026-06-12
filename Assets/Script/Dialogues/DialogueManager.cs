using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI")]
    public GameObject dialoguePanel;
    [SerializeField] private GameObject horseBubble;
    [SerializeField] private GameObject catBubble;
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI dialogueText;

    [Header("Typing Settings")]
    public float typingSpeed; //0.06f
    public float fastTypingSpeed; //0.02f
    public float defaultTypingSpeed; //0.06f
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip dialogueSound;

    private List<DialogueLine> currentLines;
    private int index;

    private Coroutine typingCoroutine;
    private bool isTyping;
    public bool isInDialogue;
    public bool openShop;
    public bool skipIncTuto;
    [SerializeField] private bool tutoDialogue;

    private void Awake()
    {
        Instance = this;
        dialoguePanel.SetActive(false);
    }

    public void StartDialogue(DialogueData data, bool isHorse)
    {
        isInDialogue = true;
        Character.Instance.canMove = false;
        Character.Instance.canMoveCam = false;
        Character.Instance.stopChara = true;
        dialoguePanel.SetActive(true);
        if(isHorse)
        {
            horseBubble.SetActive(true);
        }
        else
        {
            catBubble.SetActive(true);
            if (Shop.Instance != null && Shop.Instance.MarchandController != null)
            {
                Shop.Instance.MarchandController.AskEmote(EmoteType.Discussion);
            }
        }
        speakerText.text = data.speakerName;

        currentLines = data.lines;
        index = 0;

        ShowLine();
    }
    // public void StartDialogue(DialogueData data)
    // {
    //     isInDialogue = true;
    //     Character.Instance.canMove = false;
    //     Character.Instance.canMoveCam = false;
    //     Character.Instance.stopChara = true;
    //     dialoguePanel.SetActive(true);
    //     speakerText.text = data.speakerName;

    //     currentLines = data.lines;
    //     index = 0;

    //     ShowLine();
    // }

    public IEnumerator WaitForEndOfDialogue(DialogueData dialogue, UnityEngine.Vector3 tpPos)
    {
        StartDialogue(dialogue, false);
        yield return new WaitUntil(() => !isInDialogue);
        Debug.Log("tp");
        UiFadeManager.Instance.FadeTp(tpPos);
    }

    public void NextLine()
    {
        index++;

        if (index >= currentLines.Count)
        {
            EndDialogue();
            return;
        }

        ShowLine();
    }

    void ShowLine()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeLine(currentLines[index].text));
    }

    IEnumerator TypeLine(string line)
    {
        bool hasPlayed = false;
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in line)
        {
            dialogueText.text += letter;
            if(hasPlayed) audioSource.PlayOneShot(dialogueSound);
            hasPlayed = !hasPlayed;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        currentLines = null;

        bool wasCatTalking = catBubble.activeSelf;
        bool willOpenShop = openShop;

        if (openShop)
        {
            openShop = false;
            Shop.Instance.OpenShop(true);
        }
        else if (TutoManager.Instance.dialogueCounter != 0)
        {
            Character.Instance.canMove = true;
            Character.Instance.canMoveCam = true;
            Character.Instance.stopChara = false;
        }

        if (wasCatTalking)
        {
            Shop.Instance.MarchandController.AskEmote(EmoteType.Idle);
        }

        if (TutoManager.Instance.tuto)
        {
            if (!skipIncTuto) { TutoManager.Instance.dialogueCounter++; }
            else { skipIncTuto = false; }
            TutoManager.Instance.endOfDialogue = true;
        }

        catBubble.SetActive(false);
        horseBubble.SetActive(false);

        isInDialogue = false;
    }

    private void Update()
    {
        if (!isInDialogue)
            return;

        if (Keyboard.current.spaceKey.isPressed && isTyping && PauseManager.Instance.canPause)
        {
            
                /*StopCoroutine(typingCoroutine);
                dialogueText.text = currentLines[index].text;
                isTyping = false;*/
             typingSpeed = fastTypingSpeed;
            
            //else
            //{
            //    NextLine();
            //    typingSpeed = defaultTypingSpeed;
            //}
        }
        else { typingSpeed = defaultTypingSpeed; }

        if (Keyboard.current.spaceKey.wasPressedThisFrame && !isTyping && PauseManager.Instance.canPause)
        {
            NextLine();
            typingSpeed = defaultTypingSpeed;
        }
    }

    public void AppendText(string extraText)
    {
        if (isTyping)
        {
            // on attend que la ligne actuelle ait fini
            StartCoroutine(AppendAfterTyping(extraText));
        }
        else
        {
            // sinon on écrit directement
            dialogueText.text += extraText;
        }
    }

    private IEnumerator AppendAfterTyping(string extraText)
    {
        // attendre la fin du typing
        yield return new WaitUntil(() => !isTyping);
        isTyping = true;

        foreach (char letter in extraText)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }
}