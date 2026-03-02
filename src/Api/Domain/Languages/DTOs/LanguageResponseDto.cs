using Api.Domain.Shared.DTOs;

namespace Api.Domain.Languages.DTOs;

public class LanguageResponseDto : CreatableEntityResponseDto
{
    public required string Name { get; set; }
    public required string Family { get; set; }
    public string Script { get; set; } = "";
    public string TypicalSpeakers { get; set; } = "";
}