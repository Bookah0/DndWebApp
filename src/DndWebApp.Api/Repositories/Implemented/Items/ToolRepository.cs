using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Implemented.Items;

public class ToolRepository : IToolRepository
{
    private readonly AppDbContext context;

    public ToolRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<Tool> CreateAsync(Tool entity)
    {
        await context.Tools.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<ICollection<Tool>> GetAllAsync() => await context.Tools.ToListAsync();
    public async Task<Tool?> GetByIdAsync(int id) => await context.Tools.FirstOrDefaultAsync(t => t.Id == id);

    public async Task DeleteAsync(Tool entity)
    {
        context.Tools.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Tool updatedEntity)
    {
        context.Tools.Update(updatedEntity);
        await context.SaveChangesAsync();
    }

    public async Task<Tool?> GetWithAllDataAsync(int id)
    {
        return await context.Tools
            .Include(t => t.Properties)
            .Include(t => t.Activities)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<Tool>> GetAllWithAllDataAsync()
    {
        return await context.Tools
            .Include(t => t.Properties)
            .Include(t => t.Activities)
            .ToListAsync();
    }
}