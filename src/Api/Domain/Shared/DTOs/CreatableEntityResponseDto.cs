namespace Api.Domain.Shared.DTOs;

public abstract class CreatableEntityResponseDto
{
    public int Id { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime? UpdatedAt { get; set; }
    public required Guid? CreatedBy { get; set; }
    public required bool IsHomebrew { get; set; }
    public required bool IsPublic { get; set; }
    public required bool CloningAllowed { get; set; }
    public required ICollection<CloneEventDto> CloningHistory { get; set; }
}

public class CloneEventDto
{
    public required DateTime ClonedAt { get; set; }
    public required Guid ClonedBy { get; set; }
}