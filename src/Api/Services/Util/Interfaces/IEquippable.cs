namespace Api.Services.Util.Interfaces;

public interface IEquippable
{
    string MainSlot { get; }
    string? SecondarySlot { get; }
}