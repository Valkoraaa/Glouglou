using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UiFadeManager : MonoBehaviour
{
    private Image image;
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

    public void Fade()
    {
        StartCoroutine(FadeInAndOut());
    }

    public void FadeTp(Vector3 tpPoint)
    {
        StartCoroutine(FadeInAndOut(tpPoint));
    }

    IEnumerator FadeInAndOut()
    {
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
    }

    IEnumerator FadeInAndOut(Vector3 tpPoint)
    {
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
    }

}
