using AutoMapper;
using Api.Models.DTOs.RequestDtos;
using Api.Models.DTOs.ResponseDtos;
using Api.Models.Users;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Api.Middlewares.ExceptionHandling;
using Api.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Api.Services.Implemented;

public class UserService(RoleManager<IdentityRole<Guid>> roleManager, UserManager<User> userManager) : IUserService
{
    public async Task<User> GetByEmailAsync(string email) =>
        await userManager.Users.FirstOrDefaultAsync(u => u.Email == email)
            ?? throw new KeyNotFoundException($"User with email '{email}' not found.");
    
    public async Task<User> GetByIdAsync(Guid id) =>
        await userManager.Users.FirstOrDefaultAsync(u => u.Id == id)
            ?? throw new KeyNotFoundException($"User with id '{id}' not found.");

    public async Task<User> GetByUsernameAsync(string username) =>
        await userManager.Users.FirstOrDefaultAsync(u => u.UserName == username)
            ?? throw new KeyNotFoundException($"User with username '{username}' not found.");

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

        if(!await userManager.CheckPasswordAsync(user, password.Trim()))
            throw new ArgumentException($"Invalid login credentials.");

        return user;
    }

    public async Task<User> CheckPasswordAsync(User user, string password)
    {
        if(!await userManager.CheckPasswordAsync(user, password.Trim()))
            throw new ArgumentException($"Invalid login credentials.");

        return user;
    }

    public async Task<User> CheckPasswordAsync(LoginUserRequestDto request)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(u => u.Email == request.Email || u.UserName == request.Username)
            ?? throw new KeyNotFoundException($"Invalid login credentials.");

        if(!await userManager.CheckPasswordAsync(user, request.Password.Trim()))
            throw new ArgumentException($"Invalid login credentials.");

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