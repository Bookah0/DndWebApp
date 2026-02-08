using Api.Models.DTOs.RequestDtos;
using Api.Models.Users;

namespace Api.Services.Interfaces;

public interface IUserService
{
    Task<User> GetByUsernameAsync(string username);
    Task<User> GetByEmailAsync(string email);
    Task<User> GetByIdAsync(Guid id);
    Task<ICollection<User>> GetAllAsync();
    Task<User> CreateAsync(RegisterUserRequestDto requestDto);
    Task<User> UpdateAsync(Guid userId, UpdateUserRequestDto requestDto);
    Task DeleteAsync(Guid id);
    Task<User> ValidateLoginCredentials(LoginUserRequestDto requestDto);
    Task<User> CheckPasswordAsync(Guid id, string password);
    Task<User> CheckPasswordAsync(string usernameOrEmail, string password);
    Task InitRolesAsync();
}