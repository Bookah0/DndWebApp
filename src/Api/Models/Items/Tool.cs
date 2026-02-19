using Microsoft.EntityFrameworkCore;
namespace Api.Models.Items;

public class Tool : Item
{
    public required string ToolCategory { get; set; }
    public ICollection<ToolProperty> ToolProperties { get; set; } = [];
    public ICollection<ToolActivity> Activities { get; set; } = [];
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