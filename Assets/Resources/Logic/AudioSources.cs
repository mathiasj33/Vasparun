using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AudioSources
{
    public AudioSource footStep;
    public AudioSource landing;
    public AudioSource wallrun;
    public AudioSource jetpack;
    public AudioSource laser;
    public AudioSource wallDisappear;

    public AudioSources()
    {
        AudioSource[] sources = GetAudioSources();
        footStep = sources[0];
        landing = sources[1];
        wallrun = sources[2];
        jetpack = sources[3];
        laser = sources[4];
        wallDisappear = sources[5];
    }

    public AudioSource[] GetAudioSources()
    {
        GameObject sources = GameObject.Find("AudioSources");
        return sources.GetComponents<AudioSource>();
    }
}
