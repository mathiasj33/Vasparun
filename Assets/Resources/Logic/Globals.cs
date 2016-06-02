using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

static class Globals
{
    public static float Sensitivity { get; set; } //TODO: screenshotsaturday; INDIEDB
    public static AudioSources AudioSources = new AudioSources(); // TODO: Beta etc., Camera position bei restart (selbst machen und config file); Danebenschießen: Zeit minus; Nicht benutze Assets alle löschen!!!; Framerate independent
    public static Vector3 DistanceOrigin = new Vector3(0, 0, 0);
    public static Vector3 WarpTarget;
}
