using UnityEngine;

public class TutoManager : MonoBehaviour
{
    public static TutoManager Instance;
    public bool tuto;
    [SerializeField] private DialogueData tutoDialogue;
    [SerializeField] private DialogueData tutoDialogue2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        if(tuto)
        {
            DialogueManager.Instance.StartDialogue(tutoDialogue);
        }
    }

    public void EndOfFirstDialogue()
    {
        //mouvement cam coroutine
        DialogueManager.Instance.StartDialogue(tutoDialogue2);
    }
}
