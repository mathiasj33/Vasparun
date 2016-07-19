using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class TimeFormatter
{
    public static string Format(float time)
    {
        int minutes = (int)(time / 60);
        float seconds = time % 60;
        return minutes + "m " + seconds.ToString("0.0") + "s";
    }
}

