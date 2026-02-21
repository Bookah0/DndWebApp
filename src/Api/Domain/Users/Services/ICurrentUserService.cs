using Api.Domain.Users.Models;

namespace Api.Domain.Users.Services;

public interface ICurrentUserService
{
    Guid GetCurrentUserId();
    Task<User> GetCurrentUserAsync();
    Task SetCurrentUser(User? user);
}