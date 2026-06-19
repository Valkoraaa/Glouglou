using UnityEngine;

public class rainSoundGestion : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource rainSource;
    public AudioSource musicSource;

    void OnEnable()
    {
        if (rainSource != null)
            rainSource.Play();

        if (musicSource != null)
            musicSource.Play();
    }

    void OnDisable()
    {
        if (rainSource != null)
            rainSource.Stop();

        if (musicSource != null)
            musicSource.Stop();
    }
}
