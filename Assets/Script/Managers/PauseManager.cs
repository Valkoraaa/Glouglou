using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using TMPro;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private GameObject settingsCanvas;
    [SerializeField] private GameObject mainPause;
    private bool canPause = true;

    [SerializeField] private TMP_Dropdown dropdownResolution;


    void Start()
    {
        Resolution[] resolutions = Screen.resolutions;

        List<string> options = new List<string>();
        List<Resolution> validResolutions = new List<Resolution>();

        Resolution current = Screen.currentResolution;

        foreach (Resolution res in resolutions)
        {
            // filtre : pas plus grand que l'écran actuel
            if (res.width <= current.width && res.height <= current.height)
            {
                string option = res.width + " x " + res.height;
                options.Add(option);
                validResolutions.Add(res);
            }
        }

        dropdownResolution.ClearOptions();
        dropdownResolution.AddOptions(options);

        dropdownResolution.onValueChanged.AddListener(index =>
        {
            Resolution selected = validResolutions[index];
            Screen.SetResolution(selected.width, selected.height, FullScreenMode.FullScreenWindow);
        });
    }
    void Update()
    {
        if(Keyboard.current.escapeKey.wasPressedThisFrame && canPause)
        {
            pauseCanvas.SetActive(true);
            mainPause.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Character.Instance.canMove = false;
            Character.Instance.canMoveCam = false;
            Character.Instance.stopChara = false;
            canPause = false;
        }
    }

    public void OnPlay()
    {
        settingsCanvas.SetActive(false);
        pauseCanvas.SetActive(false);
        if(DialogueManager.Instance.isInDialogue == false)
        {
            Character.Instance.canMove = true;
            Character.Instance.canMoveCam = true;
            Character.Instance.stopChara = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        canPause = true;
    }

    public void OnSettings()
    {
        mainPause.SetActive(false);
        settingsCanvas.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsCanvas.SetActive(false);
    }

    public void OnLeave()
    {
        Application.Quit();
    }

    public void ToogleDisplayMode(int index)
    {
        if(index == 0) Screen.fullScreen = true;
        else Screen.fullScreen = false;
    }
}
