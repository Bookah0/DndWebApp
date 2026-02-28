namespace Api.Domain.Items.DTOs;

public abstract class BaseItemFilterDto
{
    public string? Name { get; set; }
    public ICollection<string>? Rarity { get; set; }
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

public class ItemFilterDto : BaseItemFilterDto
{
    public ICollection<string>? ItemCategories { get; set; }
    public bool? RequiresAttunement { get; set; }
}

public class WeaponFilterDto : BaseItemFilterDto
{
    public bool? RequiresAttunement { get; set; }
    public ICollection<string>? WeaponCategory { get; set; }
    public ICollection<string>? WeaponType { get; set; }
    public ICollection<string>? Slot { get; set; }
    public ICollection<string>? Property { get; set; }
    public ICollection<string>? DamageType { get; set; }
    public int? MinRange { get; set; }
    public int? MaxRange { get; set; }
    public bool? LongRange { get; set; }
}

public class ArmorFilterDto : BaseItemFilterDto
{
    public bool? RequiresAttunement { get; set; }
    public ICollection<string>? ArmorCategory { get; set; }
    public int? MinAC { get; set; }
    public int? MaxAC { get; set; }
    public bool? StealthDisadvantage { get; set; }
    public bool? StrengthScoreRequired { get; set; }
}

public class ToolFilterDto : BaseItemFilterDto
{
    public ICollection<string>? ToolCategory { get; set; }
}