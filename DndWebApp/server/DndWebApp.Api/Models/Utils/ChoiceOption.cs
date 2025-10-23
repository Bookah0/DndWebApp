using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Models.World;

namespace DndWebApp.Api.Utils;

public abstract class Choice
{
    public int Id { get; set; }
    public required string Description { get; set; }
    public required int NumberOfChoices { get; set; }
}

public class SkillProficiencyChoice : Choice
{
    public required ICollection<Skill> Choices { get; set; }
}

public class ItemChoice : Choice
{
    public required ICollection<Item> Choices { get; set; }
}

public class AbilityIncreaseChoice : Choice
{
    public required ICollection<AbilityValue> Choices { get; set; }
}

public class ToolProficiencyChoice : Choice
{
    public required ICollection<ToolCategory> Choices { get; set; }
}

public class LanguageChoice : Choice
{
    public required ICollection<Language> Choices { get; set; }
}

public class WeaponProficiencyChoice : Choice
{
    public required ICollection<WeaponCategory> Choices { get; set; }
}

public class ArmorProficiencyChoice : Choice
{
    public required ICollection<ArmorCategory> Choices { get; set; }
}