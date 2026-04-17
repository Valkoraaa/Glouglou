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
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && DayManager.Instance.isNight)
        {
            TextInteract.Instance.txtInteract.text = "Appuyez sur Espace pour dormir";
            TextInteract.Instance.txtObject.SetActive(true);
            interactable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && DayManager.Instance.isNight)
        {
            TextInteract.Instance.txtObject.SetActive(false);
            interactable = false;
        }
    }
}