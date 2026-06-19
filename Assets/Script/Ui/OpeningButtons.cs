using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningButtons : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private AudioSource musicSource;
    [SerializeField] private GameObject loadingTxt;
    [SerializeField] private GameObject settingPage;
    void Start()
    {
        musicSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartClick()
    {
        StartCoroutine(LoadGame());
    }

    private IEnumerator LoadGame()
    {
        loadingTxt.SetActive(true);
        float startVolume = musicSource.volume;
        float time = 0f;
        while (time < 1)
        {
            time += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, time / 1);
            yield return null;
        }
        yield return SceneManager.LoadSceneAsync("Tom Terrain", LoadSceneMode.Additive);
        yield return SceneManager.LoadSceneAsync("World_Environment", LoadSceneMode.Additive);
        yield return SceneManager.LoadSceneAsync("World_Gameplay", LoadSceneMode.Additive);
        yield return SceneManager.LoadSceneAsync("World_Main", LoadSceneMode.Additive);
        yield return SceneManager.LoadSceneAsync("WorldUI", LoadSceneMode.Additive);

        yield return null;
        yield return null;
        yield return SceneManager.UnloadSceneAsync("Opening");
    }

    public void LeaveClick()
    {
        Application.Quit();
    }

    public void SettingClick()
    {
        settingPage.SetActive(!settingPage.activeSelf);
    }
    
}
