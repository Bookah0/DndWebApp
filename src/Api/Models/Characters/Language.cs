namespace Api.Models.Characters;

// Based on https://5e-bits.github.io/docs/api/
public class Language : CreatableEntity
{
    public required string Name { get; set; }
    public required string Family { get; set; }
    public string Script { get; set; } = "";
    public string TypicalSpeakers { get; set; } = "";
}