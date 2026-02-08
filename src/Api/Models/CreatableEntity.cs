using Microsoft.EntityFrameworkCore;

namespace Api.Models;

public abstract class CreatableEntity
{
    public int Id { get; set; }
    public required DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public required Guid? CreatedBy { get; set; }
    public bool IsHomebrew { get; set; } = true;
    public bool IsPublic { get; set; } = false;
    public bool CloningAllowed { get; set; } = false;
    public ICollection<CloneEvent> CloningHistory { get; set; } = [];
}

[Owned]
public class CloneEvent
{
    public required DateTime ClonedAt { get; set; }
    public required Guid ClonedBy { get; set; }
}
