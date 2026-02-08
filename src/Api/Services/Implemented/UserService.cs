using AutoMapper;
using Api.Models.DTOs.RequestDtos;
using Api.Models.DTOs.ResponseDtos;
using Api.Models.Users;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Api.Middlewares.ExceptionHandling;

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
        if(requestDto.Password != requestDto.ConfirmPassword)
            throw new ArgumentException("Passwords do not match.");

        var user = new User
        {
            UserName = requestDto.Username,
            Email = requestDto.Email,
            CreatedAt = DateTime.UtcNow,
            PasswordHash = requestDto.Password
        };

        var result = await userManager.CreateAsync(user);

        if (!result.Succeeded)
        {
            await userManager.DeleteAsync(user);
            throw new ValidationException($"Failed to register user" + string.Join(", ", result.Errors.Select(e => e.Description)));
        }
        
        await userManager.AddToRoleAsync(user, "User");
        return user;
    }

    public async Task<User> ValidateLoginCredentials(LoginUserRequestDto requestDto)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(u => u.UserName == requestDto.UsernameOrEmail || u.Email == requestDto.UsernameOrEmail)
            ?? throw new ArgumentException($"Invalid login credentials.");  
        await CheckPasswordAsync(requestDto.UsernameOrEmail, requestDto.Password);
        return user;
    }

    public async Task<User> CheckPasswordAsync(Guid id, string password)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == id)
            ?? throw new KeyNotFoundException($"User with id '{id}' not found.");

        if(!await userManager.CheckPasswordAsync(user, password))
            throw new ArgumentException($"Invalid login credentials.");

        return user;
    }

    public async Task<User> CheckPasswordAsync(string usernameOrEmail, string password)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(u => u.UserName == usernameOrEmail || u.Email == usernameOrEmail)
                    ?? throw new ArgumentException($"Invalid login credentials.");  

        if(!await userManager.CheckPasswordAsync(user, password))
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

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<User> UpdateAsync(Guid userId, UpdateUserRequestDto requestDto)
    {
        throw new NotImplementedException();
    }
}