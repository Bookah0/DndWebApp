using Microsoft.AspNetCore.Identity;

namespace DndWebApp.Api.Models.Users;

public class User : IdentityUser<Guid>
{
    public required string Role { get; set; }
    public required DateTime CreatedAt { get; set; }
}
