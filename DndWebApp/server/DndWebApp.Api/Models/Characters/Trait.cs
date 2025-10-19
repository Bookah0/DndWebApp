
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Models.Spells;
using DndWebApp.Api.Models.World;
using DndWebApp.Api.Utils;

namespace DndWebApp.Api.Models.Characters;

// Based on https://5e-bits.github.io/docs/api/
public class Trait : BenefitProvider
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required List<Race> FromRaces { get; set; }
    public bool IsHomebrew { get; set; } = false;
}