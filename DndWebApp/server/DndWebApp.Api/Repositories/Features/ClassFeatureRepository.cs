using DndWebApp.Api.Data;
using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.Features;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Features;

public class ClassFeatureRepository : IClassFeatureRepository
{
    private readonly AppDbContext context;
    private readonly IRepository<ClassFeature> baseRepo;

    public ClassFeatureRepository(AppDbContext context, IRepository<ClassFeature> baseRepo)
    {
        this.context = context;
        this.baseRepo = baseRepo;
    }

    public async Task<ClassFeature> CreateAsync(ClassFeature entity) => await baseRepo.CreateAsync(entity);
    public async Task<ClassFeature?> GetByIdAsync(int id) => await baseRepo.GetByIdAsync(id);
    public async Task<ICollection<ClassFeature>> GetAllAsync() => await baseRepo.GetAllAsync();
    public async Task UpdateAsync(ClassFeature updatedEntity) => await baseRepo.UpdateAsync(updatedEntity);
    public async Task DeleteAsync(ClassFeature entity) => await baseRepo.DeleteAsync(entity);

    public async Task<ClassFeature?> GetWithAllDataAsync(int id)
    {
        return await context.ClassFeatures
            .AsSplitQuery()
            .Include(f => f.ClassLevel)
            .Include(f => f.AbilityIncreases)
            .Include(f => f.SpellsGained)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<ClassFeature>> GetAllWithAllDataAsync()
    {
        return await context.ClassFeatures
            .AsSplitQuery()
            .Include(f => f.ClassLevel)
            .Include(f => f.AbilityIncreases)
            .Include(f => f.SpellsGained)
            .ToListAsync();
    }
}