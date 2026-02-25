using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    public bool test;
    public DialogueData dialogue;

    private void Update()
    {
        if (test)
        {
            test = false;
            Interact();
        }
    }
    public void Interact()
    {
        DialogueManager.Instance.StartDialogue(dialogue);
    }
}