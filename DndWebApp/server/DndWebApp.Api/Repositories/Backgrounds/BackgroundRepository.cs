using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Backgrounds;

public class BackgroundRepository : IBackgroundRepository
{
    private AppDbContext context;
    private IRepository<Background> baseRepo;

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

    public async Task<BackgroundDto?> GetDtoAsync(int id)
    {
        return await context.Backgrounds
            .AsNoTracking()
            .Select(r => new BackgroundDto
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                IsHomebrew = r.IsHomebrew,
            })
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<BackgroundDto>> GetAllDtosAsync()
    {
        return await context.Backgrounds
            .AsNoTracking()
            .Select(r => new BackgroundDto
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                IsHomebrew = r.IsHomebrew,
            })
            .ToListAsync();
    }
    
    public async Task<Background?> GetWithAllDataAsync(int id)
    {
        return await context.Backgrounds
            .AsSplitQuery()
            .Include(b => b.Features)
            .Include(b => b.StartingItems)
            .Include(b => b.StartingItemsOptions)
                .ThenInclude(o => o.Options)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<Background>> GetAllWithAllDataAsync()
    {
        return await context.Backgrounds
            .AsSplitQuery()
            .Include(b => b.Features)
            .Include(b => b.StartingItems)
            .Include(b => b.StartingItemsOptions)
                .ThenInclude(o => o.Options)
            .ToListAsync();
    }
}