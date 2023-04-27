using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CurrencyFormat
{
    private static string[] _names = new[]
    {
            "",
            "K",
            "M",
            "B",
            "T"
    };

    public static string Format(float number)
    {
        if (number == 0)
        {
            return "0";
        }

        number = Mathf.Round(number);
        int i = 0;

        while (i + 1 < _names.Length && number >= 1000f)
        {
            number /= 1000f;
            i++;
        }

        return number.ToString("#.##") + _names[i];
    }
}
