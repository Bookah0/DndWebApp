using Api.Domain.Users.Models;
using Microsoft.AspNetCore.Identity;

namespace Api.Domain.Users.Services;

public class CurrentUserService(
	IHttpContextAccessor httpContextAccessor,
	SignInManager<User> signInManager)
	: ICurrentUserService
{
	public Guid GetCurrentUserId()
	{
		var claims = httpContextAccessor.HttpContext?.User?.Claims;

		if (claims is null)
			Console.WriteLine("No claims found for the current user.");
		else
			Console.WriteLine($"claims: {string.Join(", ", claims.Select(c => $"{c.Type}: {c.Value}"))}");

		var value = httpContextAccessor.HttpContext?.User?
		.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
			?? throw new Exception("User is not authenticated.");

		if (!Guid.TryParse(value, out var userId))
			throw new Exception("Invalid user identifier.");

		return userId;
	}

	public async Task SetCurrentUser(User? user)
	{
		if (user is null)
		{
			await signInManager.SignOutAsync();
			return;
		}
		await signInManager.SignInAsync(user, isPersistent: false);
	}
}