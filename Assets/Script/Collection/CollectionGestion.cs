using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Cursor = UnityEngine.Cursor;

public class CollectionGestion : MonoBehaviour
{
    [SerializeField]
    private Canvas collectionCanva;

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
                CloseCollection();
            else
                OpenCollection();
        }
    }

    public void OpenCollection()
    {
        collectionCanva.gameObject.SetActive(true);
        characterRigidbody.isKinematic = true;
        Character.Instance.canMove = false;
        Character.Instance.canMoveCam = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseCollection()
    {
        collectionCanva.gameObject.SetActive(false); 
        characterRigidbody.isKinematic = false;     
        Character.Instance.canMove = true;
        Character.Instance.canMoveCam = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}