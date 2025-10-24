using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Models.Spells;
using DndWebApp.Api.Models.World;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Models.DTOs;

public class CharacterDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required int Level { get; set; }
    public required int RaceId { get; set; }
    public required int ClassId { get; set; }
    public required int? SubClassId { get; set; }
    public required int? BackgroundId { get; set; }
    public required int? Experience { get; set; }
    public required string PlayerName { get; set; }
    public required int ProficiencyBonus { get; set; }

    // Combat stats
    public required int MaxHP { get; set; }
    public required int CurrentHP { get; set; }
    public required int TempHP { get; set; }
    public required int ArmorClass { get; set; }
    public required int Initiative { get; set; }
    public required int Speed { get; set; }
    public required int MaxHitDice { get; set; }
    public required int CurrentHitDice { get; set; }
}