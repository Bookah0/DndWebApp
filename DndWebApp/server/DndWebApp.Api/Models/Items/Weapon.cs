using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.Items.Enums;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Models.Items;

public class Weapon : Item
{
    public required WeaponCategory WeaponCategories { get; set; }
    public required WeaponProperty Properties { get; set; }
    public required DamageType DamageTypes { get; set; }
    public required string DamageDice { get; set; }
    public required int Range { get; set; }

    public string VersitileDamageDice { get; set; } = "";
    public int? LongRange { get; set; }
}

[Owned]
public class WeaponProficiency
{
    public required WeaponCategory WeaponTypes { get; set; }
    public required CharacterFeature From { get; set; }
}