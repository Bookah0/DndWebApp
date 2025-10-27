namespace DndWebApp.Api.Services.Util;

public static class ValidationUtil
{
    public static void ValidateRequiredString(string str)
    {
        if (string.IsNullOrWhiteSpace(str))
        {
            throw new ArgumentException($"{nameof(str)} cannot be null, empty, or whitespace.");
        }
    }

    public static TEnum ParseEnumOrThrow<TEnum>(string enumAsString) where TEnum : struct, Enum
    {
        if (!Enum.TryParse<TEnum>(enumAsString, true, out var result))
        {
            throw new InvalidOperationException($"Could not convert {enumAsString} to Enum of type {typeof(TEnum).Name}.");
        }
        return result;
    }

    public static List<TEnum> ParseEnumsOrThrow<TEnum>(string enumsAsString) where TEnum : struct, Enum
    {
        var splitStr = enumsAsString.Split('|');
        var result = new List<TEnum>();

        foreach (var str in splitStr)
        {
            if (!Enum.TryParse<TEnum>(str, true, out var e))
            {
                throw new InvalidOperationException($"Could not convert {e} to Enum of type {typeof(TEnum).Name}.");
            }
            result.Add(e);
        }

        return result;
    }
}