namespace Api.Domain.Shared.Repositories;

public interface IRepository<T>
{
    Task<T> CreateAsync(T entity);
    Task DeleteAsync(T entity);
    Task<T> GetByIdAsync(int id);
	Task<ICollection<T>> GetAllAsync();
    Task<T> UpdateAsync(T updatedEntity);
}