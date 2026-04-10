using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tent : MonoBehaviour
{
    [SerializeField] private bool interactable;
    private TextMeshPro txtInteract;
    private GameObject txtObject;


    void Start()
    {
        txtInteract = TextInteract.Instance.GetComponent<TextMeshPro>();
        txtObject = TextInteract.Instance.GetComponent<GameObject>();
    }

    private void Update()
    {
        if (interactable && Keyboard.current.spaceKey.wasPressedThisFrame && DayManager.Instance.isNight)
        {
            UiFadeManager.Instance.FadeEndDay();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && DayManager.Instance.isNight)
        {
            txtInteract.text = "Appuyez sur Espace pour dormir";
            txtObject.SetActive(true);
            interactable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && DayManager.Instance.isNight)
        {
            txtObject.SetActive(false);
            interactable = false;
        }
    }
}