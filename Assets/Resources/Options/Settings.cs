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

    public static int GetInt(string key)
    {
        return PlayerPrefs.GetInt(key);
    }

    public static bool GetBool(string key)
    {
        return IntToBool(GetInt(key));
    }

    public static float GetFloat(string key)
    {
        return PlayerPrefs.GetFloat(key);
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
