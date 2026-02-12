namespace Api.Models.DTOs.ResponseDtos;

public class AbilityResponseDto
{
    public int Id { get; set; }
    public required string ShortName { get; set; }
    public required string FullName { get; set; }
    public required string Description { get; set; }
    public required ICollection<SkillResponseDto> Skills { get; set; }
}