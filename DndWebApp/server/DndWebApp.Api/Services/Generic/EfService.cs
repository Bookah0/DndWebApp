
using DndWebApp.Api.Data;
using DndWebApp.Api.Repositories;
using DndWebApp.Api.Services.Generic;
using Microsoft.EntityFrameworkCore;
namespace DndWebApp.Api.Services.Generic;

public class EfService<T> : IService<T, T, T>
{
    private readonly IRepository<T> repo;

    public EfService(IRepository<T> repo)
    {
        this.repo = repo;
    }

    public virtual async Task<T> CreateAsync(T entity)
    {
        if (entity is null)
            throw new NullReferenceException("Entity can't be null");

        await repo.CreateAsync(entity);
        return entity;
    }

    public virtual async Task DeleteAsync(int id)
    {
        var entity = await repo.GetByIdAsync(id);

        if (entity is null)
            throw new NullReferenceException("Entity can't be null");

        await repo.DeleteAsync(entity);
    }

    public virtual async Task<ICollection<T>> GetAllAsync()
    {
        return await repo.GetAllAsync();
    }

    public virtual async Task<T> GetByIdAsync(int id)
    {
        var entity = await repo.GetByIdAsync(id);

        if (entity is null)
            throw new NullReferenceException("Entity can't be null");
        
        return entity;
    }

    public virtual async Task UpdateAsync(T updatedEntity)
    {
        if (updatedEntity is null)
            throw new NullReferenceException("Entity can't be null");

        await repo.UpdateAsync(updatedEntity);
    }
}