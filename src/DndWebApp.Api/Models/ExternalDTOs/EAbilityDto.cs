using DndWebApp.Api.Models.ExternalDTOs;

namespace DndWebApp.Api.Models.DTOs.ExternalDtos;

public class EAbilityDto
{
    public required string index { get; set; }
    public required string name { get; set; }
    public required string full_name { get; set; }
    public required List<string> desc { get; set; }
    public required List<EIndexDto> skills { get; set; }
    public required string url { get; set; }
    public required DateTime updated_at { get; set; }
}