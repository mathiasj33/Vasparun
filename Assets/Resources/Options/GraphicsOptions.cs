using UnityEngine;

static class GraphicsOptions
{
    public static void SetResolution(string resString, int dropdownValue, bool fullscreen)
    {
        int width = int.Parse(resString.Split('x')[0]);
        int height = int.Parse(resString.Split('x')[1]);
        Screen.SetResolution(width, height, fullscreen);

        Settings.SetInt("resValue", dropdownValue);
        Settings.SetBool("fullscreen", fullscreen);
    }

    public static void SetVsync(bool vsync)
    {
        QualitySettings.vSyncCount = 1;
        Settings.SetBool("vsync", vsync);
    }

    public static void SetAntiAliasing(string aaString, int dropdownValue)
    {
        int amount = 0;
        if (aaString != "Off") amount = int.Parse(aaString.Replace("x", ""));
        QualitySettings.antiAliasing = amount;
        Settings.SetInt("aaValue", dropdownValue);
    }
}
