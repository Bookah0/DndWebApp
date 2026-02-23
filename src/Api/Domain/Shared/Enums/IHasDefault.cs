namespace Api.Domain.Shared.Enums;

public interface IHasDefault
{
    public static abstract string Default { get; }
}