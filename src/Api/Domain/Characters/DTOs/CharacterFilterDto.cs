namespace Api.Domain.Characters.DTOs;

public class CharacterFilterDto
{
	public Guid? CreatedBy { get; set; }
	public string? Name { get; set; }
	public int? MinLevel { get; set; }
	public int? MaxLevel { get; set; }
	public ICollection<int>? Class { get; set; }
	public ICollection<int>? Subclass { get; set; }
	public ICollection<int>? Race { get; set; }
	public ICollection<int>? Subrace { get; set; }
	public ICollection<int>? Background { get; set; }
	public int? MinSpeed { get; set; }
	public int? MaxSpeed { get; set; }
	public ICollection<string>? Size { get; set; }
	public ICollection<string>? Alignment { get; set; }

	public bool? IsHomebrew { get; set; }
	public bool? CloningAllowed { get; set; }
	
	public string? SortBy { get; set; }
	public bool SortDescending { get; set; } = true;
}