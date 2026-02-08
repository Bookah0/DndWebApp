using Api.Models.Users;

namespace Api.Services.Interfaces;

public interface ICurrentUserService
{
    Guid GetCurrentUserId();
    Task<User> GetCurrentUserAsync();
    Task SetCurrentUser(User? user);
}