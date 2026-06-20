using UnityEngine;

public class KeyCanvas : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBackClick()
    {
        canvas.SetActive(false);
        PauseManager.Instance.canPause = true;
        Character.Instance.canMove = true;
        Character.Instance.stopChara = false;
        Character.Instance.canMoveCam = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
