namespace DndWebApp.Api.Utils;

public class AbilityValue
{
    public required Ability Ability { get; set; }
    public required int Value { get; set; }
}

public class SavingThrowProficiency
{
    public required Skill Skill { get; set; }
    public required bool IsProficient { get; set; }
}

public class SkillProficiency
{
    public required Skill Skill { get; set; }
    public required bool IsProficient { get; set; }
}

public class SkillExpertise
{
    public required Skill Skill { get; set; }
    public required bool HasExpertise { get; set; }
}