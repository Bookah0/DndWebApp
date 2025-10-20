using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Models.World;

namespace DndWebApp.Api.Utils;

public abstract class Choice
{
    public required string Description { get; set; }
    public required int NumberOfChoices { get; set; }
}

public class SkillProficiencyChoice : Choice
{
    public required List<Skill> Choices { get; set; }
}

public class ItemChoice : Choice
{
    public required List<Item> Choices { get; set; }
}

public class AbilityIncreaseChoice : Choice
{
    public required List<AbilityValue> Choices { get; set; }
}

public class ToolProficiencyChoice : Choice
{
    public required List<ToolCategory> Choices { get; set; }
}

public class LanguageChoice : Choice
{
    public required List<Language> Choices { get; set; }
}

public class WeaponProficiencyChoice : Choice
{
    public required List<WeaponCategory> Choices { get; set; }
}

public class ArmorProficiencyChoice : Choice
{
    public required List<ArmorCategory> Choices { get; set; }
}