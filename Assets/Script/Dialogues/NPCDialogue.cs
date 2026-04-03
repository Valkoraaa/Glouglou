using UnityEngine;
using UnityEngine.InputSystem;

public class NPCDialogue : MonoBehaviour
{
    public bool test;
    public DialogueData dialogue;
    [SerializeField] private GameObject interactUI;
    private bool canInteract;
    private bool isInDialogue;
    [SerializeField] private bool isMerchant;
    [SerializeField] private bool isDirector;

    private void Update()
    {
        if (canInteract && Keyboard.current.spaceKey.wasPressedThisFrame && !DialogueManager.Instance.isInDialogue)
        {
            //test = false;
            //Interact();
            DialogueManager.Instance.openShop = isMerchant;
            DialogueManager.Instance.StartDialogue(dialogue);
            if(isDirector)
            {
                DialogueManager.Instance.AppendText(" " + DayManager.Instance.numberOfFishToCatch.ToString() + " poissons. Bonne chance !");
            }
        }
    }
    //public void Interact()
    //{
    //    DialogueManager.Instance.StartDialogue(dialogue);
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = true;
            if (!interactUI.activeSelf) { interactUI.SetActive(true); }
            //if (Keyboard.current.spaceKey.wasPressedThisFrame)
            //{
            //    DialogueManager.Instance.StartDialogue(dialogue);
            //}
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;
            if (interactUI.activeSelf) { interactUI.SetActive(false); }
        }
    }
}