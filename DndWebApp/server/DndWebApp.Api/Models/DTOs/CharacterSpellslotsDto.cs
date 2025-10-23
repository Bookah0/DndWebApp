using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Models.Spells;
using DndWebApp.Api.Models.World;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Models.DTOs;

// Might be used?
public class CharacterSpellSlotsDto
{
    public int Id { get; set; }
    public required int CharacterId { get; set; }
    public required int Lvl1 { get; set; }
    public required int Lvl2 { get; set; }
    public required int Lvl3 { get; set; }
    public required int Lvl4 { get; set; }
    public required int Lvl5 { get; set; }
    public required int Lvl6 { get; set; }
    public required int Lvl7 { get; set; }
    public required int Lvl8 { get; set; }
    public required int Lvl9 { get; set; }
}