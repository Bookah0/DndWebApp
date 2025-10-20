namespace DndWebApp.Api.Models.World;

// From https://www.dnd5eapi.co/api/2014/alignments/
public class Alignment
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Abbreviation { get; set; }
    public required string Description { get; set; }
}