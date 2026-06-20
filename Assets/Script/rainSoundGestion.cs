using UnityEngine;

public class rainSoundGestion : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource rainSource;
    public AudioSource musicSource;

    [Header("Musiques à couper")]
    public AudioSource[] musicsToCut;

    void OnEnable()
    {
        StopOtherMusics();

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

    void StopOtherMusics()
    {
        foreach (AudioSource source in musicsToCut)
        {
            if (source != null)
                source.Stop();
        }
    }
}