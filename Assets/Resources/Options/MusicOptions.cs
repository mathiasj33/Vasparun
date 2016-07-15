using System.Linq;
using UnityEngine;

static class MusicOptions
{
    private const float DefaultMusicVolume = .16f;
    private const float DefaultFootstepVolume = 1f;
    private const float DefaultLandingVolume = .5f;
    private const float DefaultWallrunVolume = .2f;
    private const float DefaultJetpackVolume = .2f;
    private const float DefaultLaserVolume = .31f;
    private const float DefaultWallDisappearVolume = 1f;

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
        float newVolume = ratio * getDefaultValue(source);
        source.volume = newVolume;
    }

    private static float getDefaultValue(AudioSource source)
    {
        switch(source.clip.name)
        {
            case "scifi": return DefaultMusicVolume;
            case "footstep": return DefaultFootstepVolume;
            case "landing": return DefaultLandingVolume;
            case "wallrun": return DefaultWallrunVolume;
            case "jetpack": return DefaultJetpackVolume;
            case "laser": return DefaultLaserVolume;
            case "wallDisappear": return DefaultWallDisappearVolume;
        }
        return 1;
    }
}

