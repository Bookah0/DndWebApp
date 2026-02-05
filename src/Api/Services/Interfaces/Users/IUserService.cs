using Api.Models.Characters;
using Api.Models.DTOs.RequestDtos.Character;

namespace Api.Services.Interfaces;

public interface IUserService
{
    Task<object> RegisterAsync(object dto);
    Task DeleteAsync(int id);
    Task<ICollection<object>> GetAllAsync();
    Task<object> GetByIdAsync(int id);
    Task<object> UpdateAsync(int id, object dto);
}