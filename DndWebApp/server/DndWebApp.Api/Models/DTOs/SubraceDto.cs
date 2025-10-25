using DndWebApp.Api.Models.Characters;

namespace DndWebApp.Api.Models.DTOs;

public class SubraceDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string GeneralDescription { get; set; }
    public required int ParentRaceId { get; set; }
}