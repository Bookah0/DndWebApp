namespace Api.Domain.Species.DTOs;

public class SpeciesFilterDto
{
	public string? Name { get; set; }
	public int? MinSpeed { get; set; }
	public int? MaxSpeed { get; set; }
	public ICollection<string>? Size { get; set; }

	public Guid? CreatedBy { get; set; }
	public bool? IsHomebrew { get; set; }
	public bool? CloningAllowed { get; set; }
	public bool SortDescending { get; set; } = true;
}

public class RaceFilterDto : SpeciesFilterDto
{

}	

public class SubraceFilterDto : SpeciesFilterDto
{
	public int? ParentRace { get; set; }
}