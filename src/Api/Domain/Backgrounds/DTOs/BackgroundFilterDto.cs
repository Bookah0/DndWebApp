using Api.Domain.Shared.Enums;

namespace Api.Domain.Backgrounds.DTOs;

public class BackgroundFilterDto
{
	public string? Name { get; set; }
	public bool? IsHomebrew { get; set; }
	public bool? CloningAllowed { get; set; }
	public Guid? CreatedBy { get; set; }
	public SortBackgroundOption? SortBy { get; set; }
	public bool SortDescending { get; set; } = false;
} 