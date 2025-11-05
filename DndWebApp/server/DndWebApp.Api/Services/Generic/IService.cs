namespace DndWebApp.Api.Services.Generic;

public interface IService<T, TC>
{
    Task<T> CreateAsync(TC entity);
    Task DeleteAsync(int id);
    Task<ICollection<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
}