using UnityEngine;

static class Globals
{
    public const string ServerURL = "http://localhost:8080";
    public static float Sensitivity { get; set; } //TODO: screenshotsaturday; INDIEDB
    public static AudioSources AudioSources = new AudioSources(); // TODO: Beta etc., Camera position bei restart (selbst machen und config file); Danebenschießen: Zeit minus; Nicht benutze Assets alle löschen!!!; Framerate independent; Tutorial level etc.; Steam. Soundtracks? Kickstarter?
    public static Vector3 DistanceOrigin = new Vector3(0, 0, 0);
    public static Vector3 WarpTarget;
}
