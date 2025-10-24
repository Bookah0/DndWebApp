using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Items;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Items;

public class ToolRepository(AppDbContext context) : EfRepository<Tool>(context), IToolRepository
{
    public async Task<Tool?> GetWithAllDataAsync(int id)
    {
        return await dbSet
            .Include(t => t.Properties)
            .Include(t => t.Activities)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<Tool>> GetAllWithAllDataAsync()
    {
        return await dbSet
            .Include(t => t.Properties)
            .Include(t => t.Activities)
            .ToListAsync();
    }
}