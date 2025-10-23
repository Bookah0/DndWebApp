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
    public required int CharacterFeatureId { get; set; }
}

[Owned]
public class ToolProperty
{
    public required string Title { get; set; }
    public required string Description { get; set; }
}

[Owned]
public class ToolActivity
{
    public required string Title { get; set; }
    public int? SkillId { get; set; }
    public int? AbilityId { get; set; }
    public required string DC { get; set; }

}