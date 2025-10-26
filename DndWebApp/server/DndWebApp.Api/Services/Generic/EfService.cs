
using DndWebApp.Api.Data;
using DndWebApp.Api.Repositories;
using Microsoft.EntityFrameworkCore;
namespace DndWebApp.Api.Services;

public class EfService<T> : IService<T, T, T>
{
    protected IRepository<T> repo;
    AppDbContext context;

    public EfService(IRepository<T> repo, AppDbContext context)
    {
        this.context = context;
        this.repo = repo;
    }

    public virtual async Task<T> CreateAsync(T entity)
    {
        if (entity is null)
            throw new NullReferenceException("Entity can't be null");

        await repo.CreateAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task DeleteAsync(int id)
    {
        var entity = await repo.GetByIdAsync(id);

        if (entity is null)
            throw new NullReferenceException("Entity can't be null");

        await repo.DeleteAsync(entity);
        await context.SaveChangesAsync();
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
        await context.SaveChangesAsync();
    }
}