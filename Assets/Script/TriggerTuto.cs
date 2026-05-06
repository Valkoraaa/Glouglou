using UnityEngine;

public class TriggerTuto : MonoBehaviour
{
    [SerializeField] private DialogueData blockDialogue;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            TutoManager.Instance.hasToBack = true;
            DialogueManager.Instance.skipIncTuto = true;
            DialogueManager.Instance.StartCoroutine(DialogueManager.Instance.WaitForEndOfDialogue(blockDialogue, new Vector3(transform.position.x + 10, transform.position.y, transform.position.z)));
            gameObject.SetActive(false);
        }
    }
}
