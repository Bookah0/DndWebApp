using DndWebApp.Api.Models.Characters;

namespace DndWebApp.Api.Models.DTOs;

public class AbilityDto
{
    public int Id { get; set; }
    public required string ShortName { get; set; }
    public required string FullName { get; set; }
    public required string Description { get; set; }
}

public class AbilityValueDto
{
    public required int AbilityId { get; set; }
    public required int Value { get; set; }
}