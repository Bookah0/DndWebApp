using Api.Domain.Users.Models;

namespace Api.Domain.Users.Services;

public interface ICurrentUserService
{
    Guid GetCurrentUserId();
    Task SetCurrentUser(User? user);
}