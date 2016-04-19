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

    public AudioSources()
    {
        AudioSource[] sources = GameObject.Find("AudioSources").GetComponents<AudioSource>();
        footStep = sources[0];
        landing = sources[1];
        wallrun = sources[2];
        jetpack = sources[3];
        laser = sources[4];
    }
}
