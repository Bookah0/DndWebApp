using DndWebApp.Api.Data;
using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.Features;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Features;

public class BackgroundFeatureRepository : IBackgroundFeatureRepository
{
    private readonly AppDbContext context;
    private readonly IRepository<BackgroundFeature> baseRepo;

    public BackgroundFeatureRepository(AppDbContext context, IRepository<BackgroundFeature> baseRepo)
    {
        this.context = context;
        this.baseRepo = baseRepo;
    }

    public async Task<BackgroundFeature> CreateAsync(BackgroundFeature entity) => await baseRepo.CreateAsync(entity);
    public async Task<BackgroundFeature?> GetByIdAsync(int id) => await baseRepo.GetByIdAsync(id);
    public async Task<ICollection<BackgroundFeature>> GetAllAsync() => await baseRepo.GetAllAsync();
    public async Task UpdateAsync(BackgroundFeature updatedEntity) => await baseRepo.UpdateAsync(updatedEntity);
    public async Task DeleteAsync(BackgroundFeature entity) => await baseRepo.DeleteAsync(entity);

    public async Task<BackgroundFeature?> GetWithAllDataAsync(int id)
    {
        return await context.BackgroundFeatures
            .AsSplitQuery()
            .Include(b => b.Background)
            .Include(b => b.AbilityIncreases)
            .Include(b => b.SpellsGained)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<BackgroundFeature>> GetAllWithAllDataAsync()
    {
        return await context.BackgroundFeatures
            .AsSplitQuery()
            .Include(b => b.Background)
            .Include(b => b.AbilityIncreases)
            .Include(b => b.SpellsGained)
            .ToListAsync();
    }
}