static class GameplayOptions
{
    public static void SetSensitivity(float sliderValue)
    {
        Globals.Sensitivity = sliderValue;
        Settings.SetFloat("sensitivity", sliderValue);
    }
}

