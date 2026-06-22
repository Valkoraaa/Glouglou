using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using TMPro;
using UnityEngine.InputSystem.LowLevel;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private bool homePage;

    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private GameObject settingsCanvas;
    [SerializeField] private GameObject mainPause;
    public bool canPause = true;

    [SerializeField] private TMP_Dropdown dropdownResolution;

    public static PauseManager Instance;
    [SerializeField] private Transform tpCamping;
    [SerializeField] private GameObject buttonCamping;


    void Start()
    {
        Instance = this;

        if(homePage) canPause = false;

        /*Resolution[] resolutions = Screen.resolutions;

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
        });*/
    }
    void Update()
    {
        if(Keyboard.current.escapeKey.wasPressedThisFrame && canPause && !Shop.Instance.shopOpen)
        {
            canPause = false;
            pauseCanvas.SetActive(true);
            mainPause.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Character.Instance.canMove = false;
            Character.Instance.canMoveCam = false;
            Character.Instance.stopChara = false;
            
            if(!TutoManager.Instance.tuto)
            {
                buttonCamping.SetActive(true);
            }
        }

        else if(!homePage && mainPause && mainPause.activeSelf && Keyboard.current.escapeKey.wasPressedThisFrame && !Shop.Instance.shopOpen)
        {
            OnPlay();
        }

        else if(settingsCanvas.activeSelf && Keyboard.current.escapeKey.wasPressedThisFrame && !Shop.Instance.shopOpen)
        {
            BackButton();
        }

        else if (!canPause && Mouse.current.leftButton.wasPressedThisFrame)
        {
            AudioManager.Instance.AudioClick(true);
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
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
        if(index == 0)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
            Screen.fullScreen = true;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            Screen.fullScreen = true;
        }
    }

    public void BackToCamping()
    {
        OnPlay();
        ThrowLasso.Instance.hasLasso();
        UiFadeManager.Instance.FadeTp(tpCamping.position);
    }

    public void BackButton()
    {
        if(!homePage)
        {
            mainPause.SetActive(true);
            settingsCanvas.SetActive(false);
        }
        else
        {
            pauseCanvas.SetActive(false);
        }
    }
}
