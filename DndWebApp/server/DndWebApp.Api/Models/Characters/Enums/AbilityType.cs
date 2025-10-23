namespace DndWebApp.Api.Models.Characters.Enums;

[Flags]
public enum AbilityType
{
    None = 0,
    Strenght = 1 << 0,
    Dexterity = 1 << 1,
    Intelligence = 1 << 2,
    Wisdom = 1 << 3,
    Charisma = 1 << 4,
    Constitution = 1 << 5,
}