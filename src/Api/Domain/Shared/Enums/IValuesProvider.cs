namespace Api.Domain.Shared.Enums;

public interface IValuesProvider
{
    public static abstract IReadOnlySet<string> Values { get; }
}