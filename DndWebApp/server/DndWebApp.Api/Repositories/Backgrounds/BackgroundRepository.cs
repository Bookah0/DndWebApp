using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Backgrounds;

public class BackgroundRepository : IBackgroundRepository
{
    private readonly AppDbContext context;
    private readonly IRepository<Background> baseRepo;

    public BackgroundRepository(AppDbContext context, IRepository<Background> baseRepo)
    {
        this.context = context;
        this.baseRepo = baseRepo;
    }

    public async Task<Background> CreateAsync(Background entity) => await baseRepo.CreateAsync(entity);
    public async Task<Background?> GetByIdAsync(int id) => await baseRepo.GetByIdAsync(id);
    public async Task<ICollection<Background>> GetAllAsync() => await baseRepo.GetAllAsync();
    public async Task UpdateAsync(Background updatedEntity) => await baseRepo.UpdateAsync(updatedEntity);
    public async Task DeleteAsync(Background entity) => await baseRepo.DeleteAsync(entity);
    
    public async Task<Background?> GetWithAllDataAsync(int id)
    {
        return await context.Backgrounds
            .AsSplitQuery()
            .Include(b => b.Features)
            .Include(b => b.StartingItems)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<Background>> GetAllWithAllDataAsync()
    {
        return await context.Backgrounds
            .AsSplitQuery()
            .Include(b => b.Features)
            .Include(b => b.StartingItems)
            .ToListAsync();
    }
}