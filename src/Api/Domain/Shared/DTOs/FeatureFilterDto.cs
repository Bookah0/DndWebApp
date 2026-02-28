namespace Api.Domain.Shared.DTOs;

public abstract class FeatureFilterDto
{
	public string? Name { get; set; }
	public bool? IsHomebrew { get; set; }
	public bool? CloningAllowed { get; set; }
	public Guid? CreatedBy { get; set; }
	public bool SortDescending { get; set; } = true;

}

public class FeatFilterDto : FeatureFilterDto
{
}

public class TraitFilterDto : FeatureFilterDto
{
	public int? Race { get; set; }
}

public class BackgroundFeatureFilterDto : FeatureFilterDto
{
	public int? Background { get; set; }
}

public class ClassFeatureFilterDto : FeatureFilterDto
{
	public int? Class { get; set; }
	public int? Level { get; set; }
	public string? SortBy { get; set; }
}