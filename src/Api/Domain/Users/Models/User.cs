using Microsoft.AspNetCore.Identity;

namespace Api.Domain.Users.Models;

public class User : IdentityUser<Guid>
{
    public required DateTime CreatedAt { get; set; }
	public bool IsPublic { get; set; } = true;
}
