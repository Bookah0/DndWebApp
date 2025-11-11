namespace  DndWebApp.Api.Models.ExternalDTOs;

public class EAlignmentDto
{
    public required string index { get; set; }
    public required string name { get; set; }
    public required string abbreviation { get; set; }
    public required string desc { get; set; }
    public required string url { get; set; }
    public required DateTime updated_at { get; set; }
}