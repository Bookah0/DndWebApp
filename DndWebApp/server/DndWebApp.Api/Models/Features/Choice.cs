using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.Characters.Enums;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Models.Spells;
using DndWebApp.Api.Models.World.Enums;

namespace DndWebApp.Api.Models.Features;

public abstract class Choice
{
    public int Id { get; set; }
    public required string Description { get; set; }
    public required int NumberOfChoices { get; set; }
}

public class SkillProficiencyChoice : Choice
{
    public required SkillType Options { get; set; }
}

public class ItemChoice : Choice
{
    public required ICollection<Item> Options { get; set; }
}

public class AbilityIncreaseChoice : Choice
{
    public required ICollection<AbilityValue> Options { get; set; }
}

public class ToolProficiencyChoice : Choice
{
    public required ToolCategory Options { get; set; }
}

public class LanguageChoice : Choice
{
    public required LanguageType Options { get; set; }
}

public class WeaponProficiencyChoice : Choice
{
    public required WeaponCategory Options { get; set; }
}

public class ArmorProficiencyChoice : Choice
{
    public required ArmorCategory Options { get; set; }
}