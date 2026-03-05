using UnityEngine;

public class Shop : MonoBehaviour
{
    public static Shop Instance;

    [Header ("References")]
    [SerializeField] private GameObject shopCanvas;
    [SerializeField] private GameObject notEnoughMoneyCanvas;


    public int playerMoney;

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

    public void OpenShop(bool open)
    {
        shopCanvas.SetActive(open);
        Cursor.visible = open;
        if(open)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            DialogueManager.Instance.isInDialogue = false;
        }
    }


    //scaling de prix et prix de base a définir //////////////////////////////////////////////////////
    public void upgradeThrowNumber()
    {
        if(playerMoney >= priceThrow)
        {
            playerMoney -= priceThrow;
            DayManager.Instance.totalThrow += 1;
            priceThrow += 50;
            OpenShop(false);
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
            OpenShop(false);
        }
        else { NotEnoughMoney(); }
    }

    public void upgradeMoney()
    {
        //to do
        if(playerMoney >= priceMoney)
        {
            playerMoney -= priceMoney;
            priceMoney += 100;
            OpenShop(false);
        }
        else { NotEnoughMoney(); }
    }

    private void NotEnoughMoney()
    {
        notEnoughMoneyCanvas.SetActive(true);
        //ajouter son
    }
}
