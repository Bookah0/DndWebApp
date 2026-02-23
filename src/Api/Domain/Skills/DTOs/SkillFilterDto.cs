namespace Api.Domain.Skills.DTOs;

public class SkillFilterDto
{
    public Guid? CreatedBy { get; set; }
    public string? Name { get; set; }
    public int? AbilityId { get; set; }
    public bool? IsHomebrew { get; set; }
    public bool? CloningAllowed { get; set; }
    public string? SortBy { get; set; } 
    public bool SortDescending { get; set; } = false;
}