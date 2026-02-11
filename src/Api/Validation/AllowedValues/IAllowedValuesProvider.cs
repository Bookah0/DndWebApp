namespace Api.Validation.AllowedValues;

public interface IAllowedValuesProvider
{
    public static abstract IReadOnlySet<string> AllowedValues { get; }
}