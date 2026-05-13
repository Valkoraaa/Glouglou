using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Cursor = UnityEngine.Cursor;

public class CollectionGestion : MonoBehaviour
{
    [SerializeField]
    private Canvas collectionCanva;
    [SerializeField] private DisplayBook displayBook;

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
                displayBook.RefreshBook();
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
        // 1. Activer le GameObject du Canvas en premier
        collectionCanva.gameObject.SetActive(true);

        // 2. S'assurer que le script DisplayBook est actif aussi
        displayBook.gameObject.SetActive(true);

        // 3. Appeler le refresh
        displayBook.RefreshBook();

        // 4. Gérer le reste
        characterRigidbody.isKinematic = true;
        DisplayMouse(true);
    }

    public void CloseCollection()
    {
        collectionCanva.gameObject.SetActive(false);
        characterRigidbody.isKinematic = false;

        // CORRECTION ICI : on passe false pour cacher la souris et libérer le perso
        DisplayMouse(false);
    }
}