using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public GameObject dialoguePanel;
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI dialogueText;

    private List<DialogueLine> currentLines;
    private int index;

    private void Awake()
    {
        Instance = this;
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
            dialoguePanel.SetActive(false);
            return;
        }

        ShowLine();
    }

    void ShowLine()
    {
        dialogueText.text = currentLines[index].text;
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            NextLine();
        }
    }
}