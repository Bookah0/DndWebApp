using Api.Models.Users;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Api.Services.Implemented;

public class CurrentUserService(
    IUserService userService, 
    IHttpContextAccessor httpContextAccessor, 
    SignInManager<User> signInManager) 
    : ICurrentUserService
{
    public Guid GetCurrentUserId() =>  
        Guid.Parse(httpContextAccessor.HttpContext?.User?
            .FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                ?? throw new Exception("User is not authenticated."));

    public async Task<User> GetCurrentUserAsync()
    {
        var userId = Guid.Parse(httpContextAccessor.HttpContext?.User?
            .FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                ?? throw new Exception("User is not authenticated."));
        return await userService.GetByIdAsync(userId);
    }

    public async Task SetCurrentUser(User? user)
    {
        if(user is null)
        {
            await signInManager.SignOutAsync();
            return;
        }
        await signInManager.SignInAsync(user, isPersistent: false);
    } 
}