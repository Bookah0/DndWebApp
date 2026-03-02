namespace Api.Domain.Shared.DTOs;

public abstract class CreateableEntityRequestDto
{
    public bool? IsPublic { get; set; }
    public bool? CloningAllowed { get; set; }
}
