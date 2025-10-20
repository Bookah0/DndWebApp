namespace DndWebApp.Api.Models.Items.Enums;

[Flags]
public enum WeaponProperty
{
    None = 0,
    Ammunition = 1 << 0,
    Finesse = 1 << 1,
    Heavy = 1 << 2,
    Light = 1 << 3,
    Loading = 1 << 4,
    Range = 1 << 5,
    Reach = 1 << 6,
    Special = 1 << 7,
    Thrown = 1 << 8,
    TwoHanded = 1 << 9,
    Versitile = 1 << 10,
    Improvised = 1 << 11,
    Silvered = 1 << 12,
    Rod = 1 << 13
}