using DndWebApp.Api.Models.Characters;

namespace DndWebApp.Api.Models.DTOs;


/// <summary>
/// Contains primitive data from the Ability class
/// </summary>
public class AbilityPrimitiveDto
{
    public int Id { get; set; }
    public required string ShortName { get; set; }
    public required string FullName { get; set; }
    public required string Description { get; set; }
}