using UnityEngine;
using UnityEngine.UI;

public class OptionsListener : MonoBehaviour
{
    public Slider sensitivitySlider;
    public Dropdown resDropdown;
    public Dropdown antialiasingDropdown;
    public Toggle fullscreenToggle;
    public Toggle vsyncToggle;
    public Slider musicSlider;
    public Slider effectsSlider;

    void Start()
    {
        Init();
    }

    private void Init()
    {
        sensitivitySlider.value = Settings.GetFloat("sensitivity", 10);

        resDropdown.value = Settings.GetInt("resValue", 0);
        antialiasingDropdown.value = Settings.GetInt("aaValue", 0);
        fullscreenToggle.isOn = Settings.GetBool("fullscreen", true);
        vsyncToggle.isOn = Settings.GetBool("vsync", true);

        effectsSlider.value = Settings.GetFloat("soundEffectsVolume", 20);
        musicSlider.value = Settings.GetFloat("musicVolume", 20);

        ApplyOptions();
    }

    public void ApplyOptions()
    {
        GameplayOptions.SetSensitivity(sensitivitySlider.value);

        GraphicsOptions.SetResolution(resDropdown.captionText.text, resDropdown.value, fullscreenToggle.isOn);
        GraphicsOptions.SetAntiAliasing(antialiasingDropdown.captionText.text, antialiasingDropdown.value);
        GraphicsOptions.SetVsync(vsyncToggle.isOn);

        ApplyMusicOptions();
    }

    public void ApplyMusicOptions()
    {
        MusicOptions.SetMusicVolume(musicSlider.value);
        MusicOptions.SetSoundEffectsVolume(effectsSlider.value);
    }
}

