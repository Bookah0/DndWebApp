using Api.Data;
using Api.Models.Items;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implemented.Items;

public class ToolRepository(AppDbContext context) : IToolRepository
{
    public async Task<Tool> GetByIdAsync(int id) => 
        await context.Tools.FirstOrDefaultAsync(t => t.Id == id)
            ?? throw new Exception($"Tool with id {id} could not be found");

    public async Task<Tool> GetWithAllDataAsync(int id) => 
        await context.Tools
            .Include(t => t.Properties)
            .Include(t => t.Activities)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Tool with id {id} could not be found");

    public async Task<ICollection<Tool>> GetAllAsync() => await context.Tools.ToListAsync();

    public async Task<ICollection<Tool>> GetAllWithAllDataAsync() => 
        await context.Tools
            .Include(t => t.Properties)
            .Include(t => t.Activities)
            .ToListAsync();

    public async Task<Tool> CreateAsync(Tool entity)
    {
        await context.Tools.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

        public async Task DeleteAsync(Tool entity)
    {
        context.Tools.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task<Tool> UpdateAsync(Tool updatedEntity)
    {
        context.Tools.Update(updatedEntity);
        await context.SaveChangesAsync();
        return updatedEntity;
    }
}