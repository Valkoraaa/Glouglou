using UnityEngine;
using UnityEngine.InputSystem;

public class PreviewManager : MonoBehaviour
{
    [SerializeField] private GameObject preview;

    void Update()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame && ThrowLasso.Instance.canThrow && !ThrowLasso.Instance.hasThrown)
        {
            preview.SetActive(true);
        }
        else if (Mouse.current.rightButton.wasReleasedThisFrame || ThrowLasso.Instance.hasThrown || !ThrowLasso.Instance.canThrow)
        {
            preview.SetActive(false);
        }
    }
}
