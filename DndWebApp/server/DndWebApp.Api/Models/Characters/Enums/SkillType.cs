namespace DndWebApp.Api.Models.Characters.Enums;

[Flags]
public enum SkillType
{
    None = 0,
    Athletics = 1 << 0,
    Acrobatics = 1 << 1,
    SleightOfHand = 1 << 2,
    Stealth = 1 << 3,
    Arcana = 1 << 4,
    History = 1 << 5,
    Investigation = 1 << 6,
    Nature = 1 << 7,
    Religion = 1 << 8,
    AnimalHandling = 1 << 9,
    Insight = 1 << 10,
    Medicine = 1 << 11,
    Perception = 1 << 12,
    Survival = 1 << 13,
    Deception = 1 << 14,
    Intimidation = 1 << 15,
    Performance = 1 << 16,
    Persuasion = 1 << 17,
}