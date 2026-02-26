using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private void Start()
    {
        if (!SceneManager.GetSceneByName("World_Environment").isLoaded)
            SceneManager.LoadScene("World_Environment", LoadSceneMode.Additive);

        if (!SceneManager.GetSceneByName("World_Gameplay").isLoaded)
            SceneManager.LoadScene("World_Gameplay", LoadSceneMode.Additive);

        /*if (!SceneManager.GetSceneByName("World_UI").isLoaded)
            SceneManager.LoadScene("World_UI", LoadSceneMode.Additive);*/
    }
}