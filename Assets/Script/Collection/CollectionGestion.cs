using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Cursor = UnityEngine.Cursor;

public class CollectionGestion : MonoBehaviour
{
    [SerializeField]
    private Canvas collectionCanva;
    [SerializeField] private DisplayScrollCollection displayScrollCollection;

    private Rigidbody characterRigidbody;

    void Start()
    {
        collectionCanva.gameObject.SetActive(false);
        characterRigidbody = Character.Instance.gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            if (collectionCanva.gameObject.activeSelf)
            {
                CloseCollection();
                DisplayMouse(false);
                displayScrollCollection.RefreshCollection();
            }

            else
            {
                OpenCollection();
                DisplayMouse(true);
            }


        }
    }

    public void DisplayMouse(bool open)
    {
        Cursor.visible = open;

        Cursor.lockState = open ? CursorLockMode.None : CursorLockMode.Locked;

        if (Character.Instance != null)
        {
            Character.Instance.canMove = !open;
            Character.Instance.canMoveCam = !open;
        }
    }

    // Dans CollectionGestion.cs
    public void OpenCollection()
    {
        collectionCanva.gameObject.SetActive(true);


        displayScrollCollection.RefreshCollection();

        characterRigidbody.isKinematic = true;
        DisplayMouse(true);
    }

    public void CloseCollection()
    {
        collectionCanva.gameObject.SetActive(false);
        characterRigidbody.isKinematic = false;

        DisplayMouse(false);
    }
}