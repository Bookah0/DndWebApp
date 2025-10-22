using DndWebApp.Api.Models.Characters;

namespace DndWebApp.Api.Models.DTOs;

/// <summary>
/// Contains primitive data from the Subrace class
/// </summary>
public class SubracePrimitiveDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string GeneralDescription { get; set; }
    public required int ParentRaceId { get; set; }
}