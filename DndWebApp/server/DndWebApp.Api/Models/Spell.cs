namespace DndWebApp.Api.Models;

public class Spell
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public List<string> HigherLevel { get; set; } = new(); // some APIs have string instead of array

    // Spellcasting Info
    public string? Range { get; set; }
    public double? RangeValue { get; set; }
    public string? RangeUnit { get; set; }
    public string? Duration { get; set; }
    public bool IsRitual { get; set; }
    public bool NeedsConcentration { get; set; }
    public string? CastingTime { get; set; }
    public string? TargetType { get; set; }
    public int Level { get; set; }

    // Components
    public bool Verbal { get; set; }
    public bool Somatic { get; set; }
    public bool Material { get; set; }
    public string? Materials { get; set; }
    public int? MaterialCost { get; set; }
    public bool MaterialsConsumed { get; set; }
    
    // Attack & Damage
    public bool AttackRoll { get; set; }
    public string? DamageRoll { get; set; }
    public List<string> DamageTypes { get; set; } = [];
    public Ability SavingThrowAbility { get; set; }

    // School
    public string? MagicSchool { get; set; }

    // Classes & Subclasses
    public List<Class> Classes { get; set; } = [];

    // Shape info (for AoE spells)
    public string? AoeShape { get; set; }
    public double? AoeSize { get; set; }
    public string? AoeSizeUnit { get; set; }
}

public class APIReference
{
    public string? Index { get; set; }
    public string? Name { get; set; }
    public string? Url { get; set; }
}

public class CastingOption
{
    public string? Type { get; set; }
    public string? DamageRoll { get; set; }
    public int? TargetCount { get; set; }
    public string? Duration { get; set; }
    public double? Range { get; set; }
    public bool? Concentration { get; set; }
    public double? ShapeSize { get; set; }
    public string? Desc { get; set; }
}

public class DocumentReference
{
    public string? Name { get; set; }
    public string? Key { get; set; }
    public string? DisplayName { get; set; }
    public PublisherReference? Publisher { get; set; }
    public GameSystemReference? GameSystem { get; set; }
    public string? Permalink { get; set; }
}

public class PublisherReference
{
    public string? Name { get; set; }
    public string? Key { get; set; }
    public string? Url { get; set; }
}

public class GameSystemReference
{
    public string? Name { get; set; }
    public string? Key { get; set; }
    public string? Url { get; set; }
}
}