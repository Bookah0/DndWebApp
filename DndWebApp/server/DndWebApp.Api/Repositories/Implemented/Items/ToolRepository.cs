using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Items;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Implemented.Items;

public class ToolRepository : IToolRepository
{
    private readonly AppDbContext context;

    public ToolRepository(AppDbContext context)
    {
        this.context = context;
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