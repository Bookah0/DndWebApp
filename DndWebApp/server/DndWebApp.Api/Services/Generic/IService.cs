namespace DndWebApp.Api.Services.Generic;

public interface IService<T, TCreate, TUpdate>
{
    Task<T> CreateAsync(TCreate entity);
    Task DeleteAsync(int id);
    Task<ICollection<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task UpdateAsync(TUpdate updatedEntity);
}