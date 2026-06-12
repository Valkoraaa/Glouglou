using System.Collections;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public static Shop Instance;

    [Header ("References")]
    [SerializeField] private GameObject shopCanvas;
    [SerializeField] private GameObject notEnoughMoneyCanvas;

    private MarchandController marchandController;
    public MarchandController MarchandController => marchandController;
    public float playerMoney;
    public float moneyMultiplier = 1;

    //a definir
    private int priceThrow = 50;
    private int priceForce = 50;
    private int priceMoney = 50;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitialiserBoutique(MarchandController marchand)
    {
        marchandController = marchand;
    }

    public void OpenShop(bool open) // true to open, false to close
    {
        shopCanvas.SetActive(open);

        Cursor.visible = open;
        if (open)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            notEnoughMoneyCanvas.SetActive(false);
            Character.Instance.canMove = true;
            Character.Instance.canMoveCam = true;
            Character.Instance.stopChara = false;
            Cursor.lockState = CursorLockMode.Locked;
            DialogueManager.Instance.isInDialogue = false;
        }
    }


    //scaling de prix et prix de base a d�finir //////////////////////////////////////////////////////

    //upgrades
    public void upgradeThrowNumber()
    {
        if (playerMoney >= priceThrow)
        {
            playerMoney -= priceThrow;
            DayManager.Instance.numberOfFailsAllowed += 1;
            priceThrow += 50;
            marchandController.AskEmote(EmoteType.SautDeJoie);
        }
        else { NotEnoughMoney(); }
    }

    public void upgradeForce()
    {
        if (playerMoney >= priceForce)
        {
            playerMoney -= priceForce;
            FishingLasso.Instance.strenght += 1;
            priceForce += 50;
            marchandController.AskEmote(EmoteType.SautDeJoie);

            OpenShop(false);

        }
        else { NotEnoughMoney(); }
    }

    public void upgradeMoney()
    {
        if(playerMoney >= priceMoney)
        {
            moneyMultiplier += 0.1f;
            playerMoney -= priceMoney;
            priceMoney += 100;
            marchandController.AskEmote(EmoteType.SautDeJoie);
            OpenShop(false);

        }
        else { NotEnoughMoney(); }
    }

    private void NotEnoughMoney()
    {
        notEnoughMoneyCanvas.SetActive(true);
        marchandController.AskEmote(EmoteType.Rire);
        //ajouter son
    }
}
