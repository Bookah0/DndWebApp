namespace Api.Domain.Items.DTOs;

public class ItemFilterDto
{
    public string? Name { get; set; }
    public ICollection<string>? Category { get; set; }
    public string? Rarity { get; set; }
    public bool? RequiresAttunement { get; set; }
    public int? MinWeight { get; set; }
    public int? MaxWeight { get; set; }
    public int? MinValue { get; set; }
    public int? MaxValue { get; set; }

    public Guid? CreatedBy { get; set; }
    public bool? IsHomebrew { get; set; }
    public bool? CloningAllowed { get; set; }

    public string? SortBy { get; set; }
    public bool SortDescending { get; set; } = true;
}

public class WeaponFilterDto : ItemFilterDto
{
    public string? WeaponCategory { get; set; }
    public string? WeaponType { get; set; }
    public string? Slot { get; set; }
    public ICollection<string>? Property { get; set; }
    public ICollection<string>? DamageType { get; set; }
    public int? MinRange { get; set; }
    public int? MaxRange { get; set; }
    public bool? LongRange { get; set; }
}

public class ArmorFilterDto : ItemFilterDto
{
    public string? ArmorCategory { get; set; }
    public int? MinAC { get; set; }
    public int? MaxAC { get; set; }
    public bool? StealthDisadvantage { get; set; }
    public bool? StrengthScoreRequired { get; set; }
}

public class ToolFilterDto : ItemFilterDto
{
    public string? ToolCategory { get; set; }
}