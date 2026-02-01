using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.RequestDtos.Character;

namespace DndWebApp.Api.Services.Interfaces;

public interface IUserService
{
    Task<object> RegisterAsync(object dto);
    Task DeleteAsync(int id);
    Task<ICollection<object>> GetAllAsync();
    Task<object> GetByIdAsync(int id);
    Task UpdateAsync(int id, object dto);
}