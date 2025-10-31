using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Items;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Items;

public class ToolRepository : IToolRepository
{
    private AppDbContext context;
    private IRepository<Tool> baseRepo;

    public ToolRepository(AppDbContext context, IRepository<Tool> baseRepo)
    {
        this.context = context;
        this.baseRepo = baseRepo;
    }

    public async Task<Tool> CreateAsync(Tool entity) => await baseRepo.CreateAsync(entity);
    public async Task<Tool?> GetByIdAsync(int id) => await baseRepo.GetByIdAsync(id);
    public async Task<ICollection<Tool>> GetAllAsync() => await baseRepo.GetAllAsync();
    public async Task UpdateAsync(Tool updatedEntity) => await baseRepo.UpdateAsync(updatedEntity);
    public async Task DeleteAsync(Tool entity) => await baseRepo.DeleteAsync(entity);    
    
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