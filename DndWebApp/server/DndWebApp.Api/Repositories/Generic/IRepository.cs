namespace DndWebApp.Api.Repositories;

public interface IRepository<T>
{
    Task<T> CreateAsync(T entity);
    Task DeleteAsync(T entity);
    Task<ICollection<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task UpdateAsync(T updatedEntity);
}