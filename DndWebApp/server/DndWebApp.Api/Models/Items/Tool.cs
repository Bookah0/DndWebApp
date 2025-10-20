using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.Items.Enums;
using Microsoft.EntityFrameworkCore;
namespace DndWebApp.Api.Models.Items;

public class Tool : Item
{
    public required ICollection<ToolProperty> Properties { get; set; }
    public ICollection<ToolActivity> Activities { get; set; } = [];
}

[Owned]
public class ToolProficiency
{
    public required ToolCategory ToolType { get; set; }
    public required CharacterFeature From { get; set; }
}

[Owned]
public class ToolProperty
{
    public required string Title { get; set; }
    public required string PropertyDescription { get; set; }
}

[Owned]
public class ToolActivity
{
    public required string ActivityDescription { get; set; }
    public Skill? SkillToCheck { get; set; }
    public Ability? AbilityToCheck { get; set; }
    public required int DC { get; set; }

}