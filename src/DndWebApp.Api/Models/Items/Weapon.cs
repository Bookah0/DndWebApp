using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.Items.Enums;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Models.Items;

public class Weapon : Item
{
    public required WeaponCategory WeaponCategory { get; set; }
    public required WeaponType WeaponType { get; set; }
    public required ICollection<WeaponProperty> Properties { get; set; }
    public required ICollection<DamageType> DamageTypes { get; set; }
    public required string DamageDice { get; set; }
    public required int Range { get; set; }

    public string VersitileDamageDice { get; set; } = "";
    public int? LongRange { get; set; }
}