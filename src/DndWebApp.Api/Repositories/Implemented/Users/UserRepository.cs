using AutoMapper;
using Api.Models.DTOs.RequestDtos;
using Api.Models.DTOs.ResponseDtos;
using Api.Models.Users;
using Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implemented;

public class UserRepository(UserManager<User> userManager, IMapper mapper) : IUserRepository
{
    public async Task<GetUserResponseDto> GetByEmailAsync(string email) =>
        mapper.Map<GetUserResponseDto>(
            await userManager.Users.FirstOrDefaultAsync(u => u.Email == email)
            ?? throw new KeyNotFoundException($"User with email '{email}' not found."));
    
    public async Task<GetUserResponseDto> GetByIdAsync(Guid id) =>
        mapper.Map<GetUserResponseDto>(
            await userManager.Users.FirstOrDefaultAsync(u => u.Id == id)
            ?? throw new KeyNotFoundException($"User with id '{id}' not found."));

    public async Task<GetUserResponseDto> GetByUsernameAsync(string username) =>
        mapper.Map<GetUserResponseDto>(
            await userManager.Users.FirstOrDefaultAsync(u => u.UserName == username)
            ?? throw new KeyNotFoundException($"User with username '{username}' not found."));

    public async Task<ICollection<GetUserResponseDto>> GetAllAsync() => mapper.Map<ICollection<GetUserResponseDto>>(userManager.Users);

    public Task<RegisterUserResponseDto> CreateAsync(RegisterUserRequestDto userDto)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityResult> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<UpdateUserResponseDto> UpdateAsync(Guid userId, UpdateUserRequestDto userDto)
    {
        throw new NotImplementedException();
    }
}