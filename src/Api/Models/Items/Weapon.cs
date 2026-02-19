using Api.Services.Util.Interfaces;
using Api.Validation.AllowedValues.Items;

namespace Api.Models.Items;

public class Weapon : Item
{
    public required string WeaponCategory { get; set; }
    public required string WeaponType { get; set; }
    public required ICollection<string> Properties { get; set; }
    public required ICollection<string> DamageTypes { get; set; }
    public required string DamageDice { get; set; }
    public required int Range { get; set; }
    public string? VersatileDamageDice { get; set; } = "";
    public int? LongRange { get; set; }
}