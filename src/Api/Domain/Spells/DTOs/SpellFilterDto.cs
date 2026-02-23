namespace Api.Domain.Spells.DTOs;

// TODO add more fields if needed
public class SpellFilterDto
{
    public Guid? CreatedBy { get; set; }
    public string? Name { get; set; }
    public int? MinLevel { get; set; }
    public int? MaxLevel { get; set; }
    public ICollection<int>? ClassId { get; set; }
    public ICollection<string>? Duration { get; set; }
    public ICollection<string>? CastingTime { get; set; }
    public ICollection<string>? MagicSchool { get; set; }
    public ICollection<string>? SpellType { get; set; }
    public ICollection<string>? TargetType { get; set; }
    public ICollection<string>? Range { get; set; }
    public int? MinRangeValue { get; set; }
    public int? MaxRangeValue { get; set; }
    public ICollection<string>? DamageType { get; set; }

    public bool? IsHomebrew { get; set; }
    public bool? CloningAllowed { get; set; }
    
    public string? SortBy { get; set; }
    public bool SortDescending { get; set; } = true;
}