using DndWebApp.Api.Models.Characters;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Models.World;

// Based on https://5e-bits.github.io/docs/api/
public class Language
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string LanguageFamily { get; set; }
    public required string Script { get; set; }
    public bool IsHomebrew { get; set; } = false;
}

[Owned]
public class LanguageProficiency
{
    public required Language Language { get; set; }
    public required CharacterFeature From { get; set; }
}