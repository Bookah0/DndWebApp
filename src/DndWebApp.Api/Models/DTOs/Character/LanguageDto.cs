namespace DndWebApp.Api.Models.DTOs;

public class LanguageDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Family { get; set; }
    public required string Script { get; set; }
    public required bool IsHomebrew { get; set; }
}