using System.Linq.Expressions;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Api.Domain.Shared.Utils;

public static class StringExtensions
{
    public static bool Contains(this string str, string? val) =>
        val != null && str.Contains(val, StringComparison.CurrentCultureIgnoreCase);
}