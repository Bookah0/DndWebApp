using System.Text;

namespace DndWebApp.Api.Services.Util;

public static class NormalizationUtil
{
    // Can use regex but NO!
    public static string NormalizeWhiteSpace(string str)
    {
        if (string.IsNullOrEmpty(str))
            return str;

        var sb = new StringBuilder();
        var lastLetterAt = -1;

        for (int i = str.Length - 1; i >= 0; i--)
        {
            if (str[i] != ' ')
            {
                lastLetterAt = i;
                break;
            }
        }

        for (int i = 0; i <= lastLetterAt; i++)
        {
            if (str[i] != ' ')
            {
                sb.Append(str[i]);
            }
            else if (i != 0 && str[i - 1] != ' ')
            {
                sb.Append(str[i]);
            }
        }
        return sb.ToString();
    }
}