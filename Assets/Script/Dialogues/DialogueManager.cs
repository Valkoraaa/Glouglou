using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI dialogueText;

    [Header("Typing Settings")]
    public float typingSpeed = 0.1f;
    public float fastTypingSpeed = 0.03f;
    public float defaultTypingSpeed = 0.1f;

    private List<DialogueLine> currentLines;
    private int index;

    private Coroutine typingCoroutine;
    private bool isTyping;
    public bool isInDialogue;
    public bool openShop;
    [SerializeField] private bool tutoDialogue;

    private void Awake()
    {
        Instance = this;
        dialoguePanel.SetActive(false);
    }

    public void StartDialogue(DialogueData data)
    {
        isInDialogue = true;
        Character.Instance.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        Character.Instance.canMove = false;
        Character.Instance.canMoveCam = false;
        dialoguePanel.SetActive(true);
        speakerText.text = data.speakerName;

        currentLines = data.lines;
        index = 0;

        ShowLine();
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
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in line)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        currentLines = null;
        isInDialogue = false;
        
        if (openShop)
        {
            openShop = false;
            Shop.Instance.OpenShop(true);
        }
        else if (TutoManager.Instance.tuto)
        {
            TutoManager.Instance.dialogueCounter++;
            TutoManager.Instance.endOfDialogue = true;
            Debug.Log("DialogueManager check tuto manager.tuto");
        }
        else
        {
            Character.Instance.canMove = true;
            Character.Instance.canMoveCam = true;
        }
    }

    private void Update()
    {
        if (!isInDialogue)
            return;

        if (Keyboard.current.spaceKey.isPressed && isTyping)
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

        if (Keyboard.current.spaceKey.wasPressedThisFrame && !isTyping)
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