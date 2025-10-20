
using DndWebApp.Api.Data;
using Microsoft.EntityFrameworkCore;
namespace DndWebApp.Api.Repositories;

public class EfRepository<T> : IRepository<T> where T : class
{
    private AppDbContext _context;
    private DbSet<T> _set;

    public EfRepository(AppDbContext context)
    {
        _context = context;
        _set = context.Set<T>();
    }

    public virtual async Task CreateAsync(T entity)
    {
        await _set.AddAsync(entity!);
    }

    public virtual async Task DeleteAsync(T entity)
    {
        _set.Remove(entity);
    }

    public virtual async Task<ICollection<T>> GetAllAsync()
    {
        return await _set.ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _set.FindAsync(id);
    }

    public virtual async Task UpdateAsync(T updatedEntity)
    {
        _set.Update(updatedEntity);
    }
}