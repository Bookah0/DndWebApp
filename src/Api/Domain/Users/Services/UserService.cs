using Api.Domain.Users.DTOs;
using Api.Domain.Users.Models;
using Api.Infrastructure.Middleware.ExceptionHandling;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Api.Domain.Users.Services;

public class UserService(ICurrentUserService currentUserService, RoleManager<IdentityRole<Guid>> roleManager, UserManager<User> userManager) : IUserService
{
	public async Task<User> GetByEmailAsync(string email){
		var user = await userManager.Users.FirstOrDefaultAsync(u => u.Email == email)
			?? throw new KeyNotFoundException($"User with email '{email}' not found.");
	
		if(!user.IsPublic && currentUserService.GetCurrentUserId() != user.Id)
			throw new UnauthorizedAccessException($"User with email '{email}' is not public.");

		return user;
	}
	
	public async Task<User> GetByUsernameAsync(string username)
	{
		var user = await userManager.Users.FirstOrDefaultAsync(u => u.UserName == username)
			?? throw new KeyNotFoundException($"User with username '{username}' not found.");

		if(!user.IsPublic && currentUserService.GetCurrentUserId() != user.Id)
			throw new UnauthorizedAccessException($"User with username '{username}' is not public.");

		return user;
	}

	public async Task<User> GetByIdAsync(Guid id) =>
		await userManager.Users.FirstOrDefaultAsync(u => u.Id == id)
			?? throw new KeyNotFoundException($"User with id '{id}' not found.");


	public async Task<ICollection<User>> GetAllAsync() => await userManager.Users.ToListAsync();

	public async Task<User> CreateAsync(RegisterUserRequestDto requestDto)
	{
		var user = new User
		{
			UserName = requestDto.Username,
			Email = requestDto.Email,
			CreatedAt = DateTime.UtcNow
		};

		var result = await userManager.CreateAsync(user, requestDto.Password.Trim());

		if (!result.Succeeded)
		{
			await userManager.DeleteAsync(user);
			throw new ValidationException($"Failed to register user" + string.Join(", ", result.Errors.Select(e => e.Description)));
		}

		await userManager.AddToRoleAsync(user, "User");
		return user;
	}

	public async Task<User> CheckPasswordAsync(Guid id, string password)
	{
		var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == id)
			?? throw new KeyNotFoundException($"User with id '{id}' not found.");

		if (!await userManager.CheckPasswordAsync(user, password.Trim()))
			throw new ArgumentException($"Invalid login credentials.");

		return user;
	}

	public async Task<User?> CheckPasswordAsync(LoginUserRequestDto request)
	{
		var user = await userManager.Users.FirstOrDefaultAsync(u => u.Email == request.Email || u.UserName == request.Username);

		if (user is null || !await userManager.CheckPasswordAsync(user, request.Password.Trim()))
			return null;

		return user;
	}

	public async Task InitRolesAsync()
	{
		ICollection<string> roleNames = ["Admin", "User", "Moderator"];
		foreach (var roleName in roleNames)
		{
			if (!await roleManager.RoleExistsAsync(roleName))
			{
				await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
			}
		}
	}

	public async Task DeleteAsync(Guid id)
	{
		var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == id)
			?? throw new KeyNotFoundException($"User with id '{id}' not found.");
		await userManager.DeleteAsync(user);
	}

	public Task<User> UpdateAsync(Guid userId, UpdateUserRequestDto requestDto)
	{
		throw new NotImplementedException();
	}
}