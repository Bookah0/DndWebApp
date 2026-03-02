using System.ComponentModel.DataAnnotations;
using Api.Domain.Shared.Enums.Items;

namespace Api.Domain.Items.DTOs;

public class CreateToolRequestDto : CreateItemRequestBaseDto
{
    [MinLength(1)]
    [MaxLength(100)]
    public required string ToolCategory { get; set; }
}

public class UpdateToolRequestDto : UpdateItemRequestBaseDto
{
	[MinLength(1)]
    [MaxLength(100)]
    public string? ToolCategory { get; set; }
}

public class ToolPropertyDto
{
    [MinLength(1)]
    [MaxLength(100)]
    public required string Title { get; set; }

    [MinLength(1)]
    [MaxLength(2000)]
    public required string Description { get; set; }

}

public class ToolActivityDto
{
    [MinLength(1)]
    [MaxLength(100)]
    public required string Title { get; set; }

    [Range(1, int.MaxValue)]
    public int? SkillId { get; set; }

    [Range(1, int.MaxValue)]
    public int? AbilityId { get; set; }

    [MinLength(1)]
    [MaxLength(50)]
    public required string DC { get; set; }
}