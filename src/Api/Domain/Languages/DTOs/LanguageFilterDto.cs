namespace Api.Domain.Languages.DTOs;

public class LanguageFilterDto
{
  public string? Name { get; set; }
  public string? Script { get; set; }
  public string? Family { get; set; }
  
  public bool? IsHomebrew { get; set; }
  public Guid? CreatedBy { get; set; }
  public bool? CloningAllowed { get; set; }
  
  public string? SortBy { get; set; }
  public bool SortDescending { get; set; } = false;

}