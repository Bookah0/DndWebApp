using System.Text;

namespace DndWebApp.Api.Services.Util;

public static class NormalizationUtil
{
    public static string? NormalizeWhiteSpace(string str)
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

        if (lastLetterAt == -1)
        {
            return "";
        }
        
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] != ' ')
            {
                sb.Append(str[i]);

                if (i == lastLetterAt)
                {
                    return sb.ToString();
                }
            }
            else if (i != 0 && i != str.Length && str[i - 1] != ' ')
            {
                sb.Append(str[i]);
            }
        }
        return null;
    }
}