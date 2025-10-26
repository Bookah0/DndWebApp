namespace DndWebApp.Api.Services.Utils;

public static class ValidationUtil
{
    public static void ValidateRequired(string str, string varName)
    {
        if (string.IsNullOrWhiteSpace(str))
        {
            throw new ArgumentException($"{varName} cannot be null, empty, or whitespace.");
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
}