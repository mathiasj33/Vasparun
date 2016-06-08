using System.Linq;
using UnityEngine;

static class MusicOptions
{
    public static void SetMusicVolume(float sliderValue)
    {
        AudioSource source = GameObject.Find("Music").GetComponent<AudioSource>();
        SetVolume(source, sliderValue);
        Settings.SetFloat("musicVolume", sliderValue);
    }

    public static void SetSoundEffectsVolume(float sliderValue)
    {
        AudioSource[] sources = Globals.AudioSources.GetAudioSources();
        sources.ToList<AudioSource>().ForEach(s => SetVolume(s, sliderValue));
        Settings.SetFloat("soundEffectsVolume", sliderValue);
    }

    private static void SetVolume(AudioSource source, float sliderValue)
    {
        float ratio = sliderValue / 20;
        float newVolume = ratio * source.volume;
        source.volume = newVolume;
    }
}

