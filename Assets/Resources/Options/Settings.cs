using UnityEngine;

static class Settings
{
    public static void SetInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }

    public static void SetBool(string key, bool value)
    {
        SetInt(key, BoolToInt(value));
    }

    public static void SetFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }

    public static void SetString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }

    public static int GetInt(string key, int defaultValue)
    {
        return PlayerPrefs.GetInt(key, defaultValue);
    }

    public static bool GetBool(string key, bool defaultValue)
    {
        return IntToBool(GetInt(key, BoolToInt(defaultValue)));
    }

    public static float GetFloat(string key, float defaultValue)
    {
        return PlayerPrefs.GetFloat(key, defaultValue);
    }

    public static string GetString(string key, string defaultValue)
    {
        return PlayerPrefs.GetString(key, defaultValue);
    }

    private static int BoolToInt(bool value)
    {
        if (value) return 1;
        return 0;
    }

    private static bool IntToBool(int value)
    {
        return value == 1;
    }
}
