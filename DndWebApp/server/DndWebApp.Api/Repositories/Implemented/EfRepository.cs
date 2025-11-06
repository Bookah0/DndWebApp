
using DndWebApp.Api.Data;
using DndWebApp.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Implemented;

public class EfRepository<T> : IRepository<T> where T : class
{
    private readonly AppDbContext context;
    private readonly DbSet<T> dbSet;

    public EfRepository(AppDbContext context)
    {
        this.context = context;
        dbSet = context.Set<T>();
    }

    public async Task<T> CreateAsync(T entity)
    {
        await dbSet.AddAsync(entity!);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(T entity)
    {
        dbSet.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task<ICollection<T>> GetAllAsync() => await dbSet.ToListAsync();

    public async Task<T?> GetByIdAsync(int id) => await dbSet.FindAsync(id);

    public async Task UpdateAsync(T updatedEntity)
    {
        dbSet.Update(updatedEntity);
        await context.SaveChangesAsync();
    }
}