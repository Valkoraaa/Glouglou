using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tent : MonoBehaviour
{
    [SerializeField] private bool interactable;
    


    void Start()
    {
        
    }

    private void Update()
    {
        if (interactable && Keyboard.current.spaceKey.wasPressedThisFrame && DayManager.Instance.isNight)
        {
            UiFadeManager.Instance.FadeEndDay();
            TextInteract.Instance.txtInteract.gameObject.SetActive(false);
            interactable = false;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && DayManager.Instance.isNight)
        {
            TextInteract.Instance.txtInteract.text = "Appuyez sur Espace pour dormir";
            TextInteract.Instance.txtInteract.gameObject.SetActive(true);
            interactable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TextInteract.Instance.txtInteract.gameObject.SetActive(false);
            interactable = false;
        }
    }
}