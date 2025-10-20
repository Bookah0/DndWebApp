using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.Items.Enums;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Models.Items;

[Owned]
public class DamageAffinity
{
    public required AffinityType AffinityType { get; set; }
    public required DamageType DamageType { get; set; }
    public required CharacterFeature From { get; set; }
}