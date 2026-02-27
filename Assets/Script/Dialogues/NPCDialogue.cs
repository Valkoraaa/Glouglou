using UnityEngine;
using UnityEngine.InputSystem;

public class NPCDialogue : MonoBehaviour
{
    public bool test;
    public DialogueData dialogue;
    [SerializeField] private GameObject interactUI;

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

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!interactUI.activeSelf) { interactUI.SetActive(true); }
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                DialogueManager.Instance.StartDialogue(dialogue);
            }
        }
    }
}