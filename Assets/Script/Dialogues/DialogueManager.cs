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
    public float typingSpeed = 0.05f;

    private List<DialogueLine> currentLines;
    private int index;

    private Coroutine typingCoroutine;
    private bool isTyping;

    private void Awake()
    {
        Instance = this;
        dialoguePanel.SetActive(false);
    }

    public void StartDialogue(DialogueData data)
    {
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
    }

    private void Update()
    {
        if (!dialoguePanel.activeSelf)
            return;

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (isTyping)
            {
                /*StopCoroutine(typingCoroutine);
                dialogueText.text = currentLines[index].text;
                isTyping = false;*/
                typingSpeed = 0.02f;
            }
            else
            {
                NextLine();
                typingSpeed = 0.05f;
            }
        }
    }
}