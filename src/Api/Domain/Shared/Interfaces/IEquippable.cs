namespace Api.Domain.Shared.Interfaces;

public interface IEquippable
{
    string MainSlot { get; }
    string? SecondarySlot { get; }
}