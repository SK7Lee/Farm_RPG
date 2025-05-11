using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class AudioUIManager : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        // Gán giá trị mặc định
        musicSlider.value = 1f;
        sfxSlider.value = 1f;

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    void SetMusicVolume(float value)
    {
        if (AudioManager.instance != null)
            AudioManager.instance.SetVolume(value);
    }

    void SetSFXVolume(float value)
    {
        if (SFXManager.instance != null)
            SFXManager.instance.SetVolume(value);
    }
}
