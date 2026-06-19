using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioMixer mixerMusic;
    [SerializeField] private AudioMixer mixerEffect;
    [SerializeField] private Slider sliderMusic;
    [SerializeField] private Slider sliderEffect;
    [SerializeField] private AudioSource clickUI;
    [SerializeField] private AudioClip clickGood;
    [SerializeField] private AudioClip clickBad;

    private void Start()
    {
        Instance = this;

        float dB;

        if (mixerMusic.GetFloat("MyExposedParam", out dB))
        {
            sliderMusic.value = Mathf.Pow(10f, dB / 20f);
        }

        if (mixerEffect.GetFloat("MyExposedParam", out dB))
        {
            sliderEffect.value = Mathf.Pow(10f, dB / 20f);
        }

        sliderMusic.onValueChanged.AddListener(value => SetVolume(value, "music"));
        sliderEffect.onValueChanged.AddListener(value => SetVolume(value, "effect"));
    }

    public void SetVolume(float value, string audioType)
    {
        // conversion slider (0–1) → dB (-80 à 0)
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        if(audioType == "music") mixerMusic.SetFloat("MyExposedParam", dB);

        else mixerEffect.SetFloat("MyExposedParam", dB);
    }

    public void AudioClick(bool confirm)
    {
        if(confirm)
        {
            clickUI.PlayOneShot(clickGood);
        }
        else { clickUI.PlayOneShot(clickBad); }
        
    }
}
