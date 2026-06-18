using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UiFadeManager : MonoBehaviour
{
    private Image image;
    [SerializeField] private NPCDialogue dialogueStartOfDay;
    public static UiFadeManager Instance { get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image = GetComponentInChildren<Image>();
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeEndDay()
    {
        StartCoroutine(FadeInAndOutEndDay());
    }

    public void FadeTp(Vector3 tpPoint)
    {
        StartCoroutine(FadeInAndOut(tpPoint));
    }

    IEnumerator FadeInAndOutEndDay()
    {
        Character.Instance.gameObject.GetComponent<Rigidbody>().isKinematic = true; ///////////
        Character.Instance.canMove = false;
        Character.Instance.canMoveCam = false;
        PauseManager.Instance.canPause = false;
        float time = 0f;
        Color color = image.color;

        // Fade IN (0 → 1)
        color.a = 0f;
        image.color = color;

        while (time < 0.5f)
        {
            time += Time.deltaTime;
            color.a = time / 0.5f;
            image.color = color;

            yield return null;
        }

        color.a = 1f;
        image.color = color;
        yield return new WaitForSeconds(0.5f);
        DayManager.Instance.StartOfDay();
        // Fade OUT (1 → 0)
        time = 0f;

        while (time < 0.5f)
        {
            time += Time.deltaTime;
            color.a = 1f - (time / 0.5f);
            image.color = color;

            yield return null;
        }

        color.a = 0f;
        image.color = color;
        dialogueStartOfDay.StartDialogue();
    }

    IEnumerator FadeInAndOut(Vector3 tpPoint)
    {
        Character.Instance.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        Character.Instance.canMove = false;
        Character.Instance.canMoveCam = false;
        PauseManager.Instance.canPause = false;
        float time = 0f;
        Color color = image.color;

        // Fade IN (0 → 1)
        color.a = 0f;
        image.color = color;

        while (time < 0.5f)
        {
            time += Time.deltaTime;
            color.a = time / 0.5f;
            image.color = color;
            yield return null;
        }

        color.a = 1f;
        image.color = color;

        CharacterController controller = Character.Instance.GetComponent<CharacterController>();

        controller.enabled = false;
        Character.Instance.transform.position = tpPoint;
        controller.enabled = true;

        yield return new WaitForSeconds(0.05f);
        // Fade OUT (1 → 0)
        time = 0f;

        while (time < 0.5f)
        {
            time += Time.deltaTime;
            color.a = 1f - (time / 0.5f);
            image.color = color;

            yield return null;
        }

        color.a = 0f;
        image.color = color;
        if(TutoManager.Instance.tuto) { TutoManager.Instance.fadeFinished = true; }
        if(TutoManager.Instance.tuto && TutoManager.Instance.dialogueCounter>=3) { TutoManager.Instance.tuto = false; }
        else if (TutoManager.Instance.tuto) { TutoManager.Instance.blockActive = true; }
        else
        {
            Character.Instance.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            Character.Instance.canMove = true;
            Character.Instance.canMoveCam = true;
        }
        PauseManager.Instance.canPause = true; 
        
        
    }

}
