using DndWebApp.Api.Middlewares.ExceptionHandling;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.VisualBasic;

namespace DndWebApp.Api.Services.Util;

public static class ConstantsUtil
{
    public static ICollection<string> ResolveOptionOrThrow(ICollection<string> inputs, IReadOnlySet<string> allowedSet, string constantsGroupName)
    {
        ICollection<string> resolved = [];

        foreach (var input in inputs)
        {
            if (TryResolveOption(input, allowedSet, out var resolvedOption))
                resolved.Add(resolvedOption!);
        
            throw new NotFoundException($"{constantsGroupName} {input} not recognized.");
        }
        return resolved;
    }

    public static string ResolveOptionOrThrow(string input, IReadOnlySet<string> allowedSet, string constantsGroupName)
    {
        if (TryResolveOption(input, allowedSet, out var resolved))
            return resolved!;
        
        throw new NotFoundException($"{constantsGroupName} {input} not recognized.");
    }

    public static bool TryResolveOption(string input, IReadOnlySet<string> allowedSet, out string? resolved)
    {
        if (allowedSet.Contains(input))
        {
            resolved = input;
            return true;
        }

        var normalizedInput = Normalize(input);

        foreach (var allowed in allowedSet)
        {
            if (Normalize(allowed).Equals(normalizedInput))
            {
                resolved = allowed;
                return true;
            }
        }
        resolved = null;
        return false;
    }

    public static string Normalize(string str) {
        return str
            .Replace("-", "")
            .Replace("_", "")
            .Replace("'", "")
            .Replace(" ", "")
            .ToLower();
    }

    internal static object ResolveOptionOrThrow(string? value, object weaponCategories, string v)
    {
        throw new NotImplementedException();
    }
}