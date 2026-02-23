using Api.Domain.Shared.Enums;

namespace Api.Domain.Classes.DTOs;

public class ClassFilterDto
{
    public Guid? CreatedBy { get; set; }
    public string? Name { get; set; }
    public bool? IsSpellcaster { get; set; }
    public bool? IsHomebrew { get; set; }
    public bool? CloningAllowed { get; set; }
    public bool SortDescending { get; set; } = true;
}

public class SubclassFilterDto : ClassFilterDto
{
    public string? SortBy { get; set; } 

}