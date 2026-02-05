namespace Api.Models.DTOs.ResponseDtos;

public class LanguageResponseDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Family { get; set; }
    public required string Script { get; set; }
    public List<string> TypicalSpeakers { get; set; } = [];
    public bool IsExotic { get; set; }
    public bool IsHomebrew { get; set; }
}