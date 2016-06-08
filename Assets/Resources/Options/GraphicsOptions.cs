using UnityEngine;

static class GraphicsOptions
{
    public static void SetResolution(string resString, bool fullscreen)
    {
        int width = int.Parse(resString.Split('x')[0]);
        int height = int.Parse(resString.Split('x')[1]);
        Screen.SetResolution(width, height, fullscreen);

        Settings.SetInt("width", width);
        Settings.SetInt("height", height);
        Settings.SetBool("fullscreen", fullscreen);
    }

    public static void SetVsync(bool vsync)
    {
        QualitySettings.vSyncCount = 1;
        Settings.SetBool("vsync", vsync);
    }

    public static void SetAntiAliasing(int amount)
    {
        QualitySettings.antiAliasing = amount;
        Settings.SetInt("antiAliasing", amount);
    }
}
