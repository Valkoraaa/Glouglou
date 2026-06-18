using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public static GameOver Instance;
    [SerializeField] private GameObject canvasGameOver;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
    }

    public void OnGameOver()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Character.Instance.canMove = false;
        Character.Instance.canMoveCam = false;
        Character.Instance.stopChara = false;
        PauseManager.Instance.canPause = false;
        canvasGameOver.SetActive(true);
    }


    public void Leave()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        StartCoroutine(LoadMainMenu());
    }

    private IEnumerator LoadMainMenu()
    {
        yield return SceneManager.LoadSceneAsync("Opening", LoadSceneMode.Additive);


        yield return SceneManager.UnloadSceneAsync("Tom Terrain");
        yield return SceneManager.UnloadSceneAsync("World_Environment");
        yield return SceneManager.UnloadSceneAsync("World_Gameplay");
        yield return SceneManager.UnloadSceneAsync("World_Main");
        yield return SceneManager.UnloadSceneAsync("WorldUI");
    }
}
