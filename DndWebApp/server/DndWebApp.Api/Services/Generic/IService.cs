namespace DndWebApp.Api.Services.Generic;

public interface IService<T, CT, UT>
{
    Task<T> CreateAsync(CT entity);
    Task DeleteClassLevelAsync(int id);
    Task<ICollection<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task UpdateAsync(UT updatedEntity);
}