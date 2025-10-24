using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Backgrounds;

public class BackgroundRepository(AppDbContext context) : EfRepository<Background>(context), IBackgroundRepository
{
    public async Task<BackgroundPrimitiveDto?> GetPrimitiveDataAsync(int id)
    {
        return await dbSet
            .AsNoTracking()
            .Select(r => new BackgroundPrimitiveDto
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                IsHomebrew = r.IsHomebrew,
            })
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<BackgroundPrimitiveDto>> GetAllPrimitiveDataAsync()
    {
        return await dbSet
            .AsNoTracking()
            .Select(r => new BackgroundPrimitiveDto
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
        return await dbSet
            .AsSplitQuery()
            .Include(b => b.Features)
            .Include(b => b.StartingItems)
            .Include(b => b.StartingItemsOptions)
                .ThenInclude(o => o.Options)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<Background>> GetAllWithAllDataAsync()
    {
        return await dbSet
            .AsSplitQuery()
            .Include(b => b.Features)
            .Include(b => b.StartingItems)
            .Include(b => b.StartingItemsOptions)
                .ThenInclude(o => o.Options)
            .ToListAsync();
    }
}