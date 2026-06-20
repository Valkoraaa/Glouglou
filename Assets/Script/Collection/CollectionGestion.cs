using TMPro;
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

    [SerializeField] private Image fishImage;
    [SerializeField] private TMP_Text fishName;
    [SerializeField] private TMP_Text fishSize;
    [SerializeField] private TMP_Text fishWeight;

    private Rigidbody characterRigidbody;

    void Start()
    {
        collectionCanva.gameObject.SetActive(false);
        characterRigidbody = Character.Instance.gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Keyboard.current.tabKey.wasPressedThisFrame && PauseManager.Instance.canPause && !DialogueManager.Instance.isInDialogue && !Character.Instance.cinematic)
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

    public void DisplayFishInfo(FishData data)
    {
        fishImage.sprite = data.icon;
        fishName.text = "Nom : " + data.species;
        fishSize.text = "Taille : " + FishingBookManager.Instance.GetBestSize(data.id).ToString("F2") + " cm";
        fishWeight.text = "Poids : " + FishingBookManager.Instance.GetBestWeight(data.id).ToString("F2") + " kg";
    }
}