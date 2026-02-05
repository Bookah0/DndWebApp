using DndWebApp.Api.Models.DTOs.RequestDtos;
using DndWebApp.Api.Models.DTOs.ResponseDtos;
using DndWebApp.Api.Models.Users;
using DndWebApp.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DndWebApp.Api.Repositories.Interfaces;

public interface IUserRepository
{
    Task<GetUserResponseDto> GetByUsernameAsync(string username);
    Task<GetUserResponseDto> GetByEmailAsync(string email);
    Task<GetUserResponseDto> GetByIdAsync(Guid id);
    Task<ICollection<GetUserResponseDto>> GetAllAsync();
    Task<RegisterUserResponseDto> CreateAsync(RegisterUserRequestDto userDto);
    Task<UpdateUserResponseDto> UpdateAsync(Guid userId, UpdateUserRequestDto userDto);
    Task<IdentityResult> DeleteAsync(Guid id);
}