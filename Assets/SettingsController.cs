using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;
    public GameObject settingsScreen;
    public GameObject menuScreen;

    private void Start()
    {
        // Khởi tạo music slider
        if (AudioManager.instance != null)
        {
            musicSlider.value = AudioManager.instance.GetVolume();
            musicSlider.onValueChanged.AddListener(UpdateMusicVolume);
        }

        // Khởi tạo sfx slider
        if (SFXManager.instance != null)
        {
            sfxSlider.value = SFXManager.instance.GetVolume();
            sfxSlider.onValueChanged.AddListener(UpdateSFXVolume);
        }
    }

    public void UpdateMusicVolume(float value)
    {
        if (AudioManager.instance != null)
            AudioManager.instance.SetVolume(value);
    }

    public void UpdateSFXVolume(float value)
    {
        if (SFXManager.instance != null)
            SFXManager.instance.SetVolume(value);
    }

    
}
