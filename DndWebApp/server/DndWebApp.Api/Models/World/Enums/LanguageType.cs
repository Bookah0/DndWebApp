namespace DndWebApp.Api.Models.World.Enums;

[Flags]
public enum LanguageType
{
    None = 0,
    Common = 1 << 0,
    Dwarvish = 1 << 1,
    Elvish = 1 << 2,
    Giant = 1 << 3,
    Gnomish = 1 << 4,
    Goblin = 1 << 5,
    Halfling = 1 << 6,
    Orc = 1 << 7,
    Abyssal = 1 << 8,
    Celestial = 1 << 9,
    Draconic = 1 << 10,
    DeepSpeech = 1 << 11,
    Infernal = 1 << 12,
    Primordial = 1 << 13,
    Undercommon = 1 << 14,
}