namespace DndWebApp.Api.Repositories.Interfaces;

public interface IRepository<T>
{
    Task<T> CreateAsync(T entity);
    Task DeleteAsync(T entity);
    Task<ICollection<T>> GetMiscellaneousItemsAsync();
    Task<T?> GetByIdAsync(int id);
    Task UpdateAsync(T updatedEntity);
}