using DndWebApp.Api.Data;
using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.Features;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Features;

public class TraitRepository : ITraitRepository
{
    private readonly AppDbContext context;
    private readonly IRepository<Trait> baseRepo;

    public TraitRepository(AppDbContext context, IRepository<Trait> baseRepo)
    {
        this.context = context;
        this.baseRepo = baseRepo;
    }

    public async Task<Trait> CreateAsync(Trait entity) => await baseRepo.CreateAsync(entity);
    public async Task<Trait?> GetByIdAsync(int id) => await baseRepo.GetByIdAsync(id);
    public async Task<ICollection<Trait>> GetAllAsync() => await baseRepo.GetAllAsync();
    public async Task UpdateAsync(Trait updatedEntity) => await baseRepo.UpdateAsync(updatedEntity);
    public async Task DeleteAsync(Trait entity) => await baseRepo.DeleteAsync(entity);

    public async Task<Trait?> GetWithAllDataAsync(int id)
    {
        return await context.Traits
            .AsSplitQuery()
            .Include(t => t.FromRace)
            .Include(f => f.AbilityIncreases)
            .Include(f => f.SpellsGained)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<Trait>> GetAllWithAllDataAsync()
    {
        return await context.Traits
            .AsSplitQuery()
            .Include(t => t.FromRace)
            .Include(f => f.AbilityIncreases)
            .Include(f => f.SpellsGained)
            .ToListAsync();
    }
}