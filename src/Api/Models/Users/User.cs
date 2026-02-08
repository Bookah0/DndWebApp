using Microsoft.AspNetCore.Identity;

namespace Api.Models.Users;

public class User : IdentityUser<Guid>
{
    public string Role { get; set; } = "";
    public required DateTime CreatedAt { get; set; }
}
