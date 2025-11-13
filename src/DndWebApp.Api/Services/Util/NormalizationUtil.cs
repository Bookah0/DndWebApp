using System.Text;

namespace DndWebApp.Api.Services.Util;

public static class NormalizationUtil
{
        public static TEnum ParseEnumOrThrow<TEnum>(string? enumAsString) where TEnum : struct, Enum
    {
        if (enumAsString is null)
            return default;

        var formattedString = NormalizationUtil.ToEnumPascalCaseFormat(enumAsString);

        if (!Enum.TryParse<TEnum>(formattedString, true, out var result))
        {
            throw new InvalidOperationException($"Could not convert {enumAsString} to Enum of type {typeof(TEnum).Name}. Formatted string: {formattedString}");
        }
        return result;
    }

    public static List<TEnum> ParseEnumOrThrow<TEnum>(ICollection<string>? enumStrings) where TEnum : struct, Enum
    {
        if (enumStrings is null)
            return [];

        return [.. enumStrings.Select(ParseEnumOrThrow<TEnum>)];
    }

    public static string ToEnumPascalCaseFormat(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        string normalized = input
            .Replace("-", " ")
            .Replace("_", " ");

        var words = normalized
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);

        var pascalCase = string.Concat(words.Select(word =>
            char.ToUpper(word[0]) + word[1..].ToLower()));

        return pascalCase;
    }

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