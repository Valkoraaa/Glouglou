using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UiFadeManager : MonoBehaviour
{
    private Image image;
    [SerializeField] private NPCDialogue dialogueStartOfDay;
    public Material skyboxJour;
    public Material skyboxNuit;
    [SerializeField] private Light myLight;


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
        RenderSettings.skybox = skyboxJour;
        UnityEngine.ColorUtility.TryParseHtmlString("#FFF0C4", out Color yellow);
        StartCoroutine(UiFadeManager.Instance.LerpLightColor(yellow, 0.5f));

        //Debug.Log(lightInstance.Instance.light.color);


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

        Character.Instance.gameObject.GetComponent<Rigidbody>().isKinematic = false; ///////////
        Character.Instance.canMove = true;
        Character.Instance.canMoveCam = true;
        PauseManager.Instance.canPause = true;
        //dialogueStartOfDay.StartDialogue();
    }

    IEnumerator FadeInAndOut(Vector3 tpPoint)
    {
        Character.Instance.canMove = false;
        Character.Instance.canMoveCam = false;
        PauseManager.Instance.canPause = false;
        Character.Instance.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        
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
        if (DayManager.Instance.isNight)
        {
            RenderSettings.skybox = skyboxNuit;
            ColorUtility.TryParseHtmlString("#000000", out Color black);
            StartCoroutine(LerpLightColor(black, 0.5f));
        }
        else
        {
            RenderSettings.skybox = skyboxJour;
            ColorUtility.TryParseHtmlString("#FFF0C4", out Color yellow);
            StartCoroutine(LerpLightColor(yellow, 0.5f));
        }
        
        



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
        
        Character.Instance.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        Character.Instance.canMove = true;
        Character.Instance.canMoveCam = true;
        
        PauseManager.Instance.canPause = true; 
        
        
    }

    public IEnumerator LerpLightColor(Color targetColor, float duration)
    {
        Color startColor = lightInstance.Instance.light.color;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            lightInstance.Instance.light.color = Color.Lerp(startColor, targetColor, time / duration);
            yield return null;
        }

        lightInstance.Instance.light.color = targetColor;
    }

}
