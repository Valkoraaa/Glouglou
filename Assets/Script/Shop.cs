using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private GameObject shopCanvas;
    public static Shop Instance;
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
        else { Cursor.lockState = CursorLockMode.Locked; }
    }
}
