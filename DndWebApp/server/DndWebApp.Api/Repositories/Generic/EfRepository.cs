
using DndWebApp.Api.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
namespace DndWebApp.Api.Repositories;

public class EfRepository<T> : IRepository<T> where T : class
{
    protected AppDbContext context;
    protected DbSet<T> dbSet;

    public EfRepository(AppDbContext context)
    {
        this.context = context;
        dbSet = context.Set<T>();
    }

    public virtual async Task<T> CreateAsync(T entity)
    {
        await dbSet.AddAsync(entity!);
        return entity;
    }

    public virtual async Task DeleteAsync(T entity)
    {
        await Task.Run(() => dbSet.Remove(entity));
    }

    public virtual async Task<ICollection<T>> GetAllAsync()
    {
        return await dbSet.ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await dbSet.FindAsync(id);
    }

    public virtual async Task UpdateAsync(T updatedEntity)
    {
        await Task.Run(() => dbSet.Update(updatedEntity));
    }
}