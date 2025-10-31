using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.Characters.Enums;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Models.Spells;
using DndWebApp.Api.Models.World.Enums;

namespace DndWebApp.Api.Models.Features;

public abstract class Option
{
    public int Id { get; set; }
    public required string Description { get; set; }
    public required int NumberOfChoices { get; set; }
}

public class SkillProficiencyOption : Option
{
    public required ICollection<SkillType> Options { get; set; }
}

public class ItemOption : Option
{
    public required ICollection<Item> Options { get; set; }
}

public class AbilityIncreaseOption : Option
{
    public required ICollection<AbilityValue> Options { get; set; }
}

public class ToolProficiencyOption : Option
{
    public required ICollection<ToolCategory> Options { get; set; }
}

public class LanguageOption : Option
{
    public required ICollection<LanguageType> Options { get; set; }
}

public class WeaponProficiencyOption : Option
{
    public required ICollection<WeaponCategory> CategoryOptions { get; set; }
    public required ICollection<WeaponType> TypeOptions { get; set; }
}

public class ArmorProficiencyOption : Option
{
    public required ICollection<ArmorCategory> Options { get; set; }
}