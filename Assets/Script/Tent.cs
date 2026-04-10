using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tent : MonoBehaviour
{
    [SerializeField] private GameObject uiKeyToPress;
    [SerializeField] private bool interactable;

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
            uiKeyToPress.SetActive(true);
            interactable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && DayManager.Instance.isNight)
        {
            uiKeyToPress.SetActive(false);
            interactable = false;
        }
    }
}
