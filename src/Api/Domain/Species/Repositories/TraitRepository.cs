using Api.Domain.Shared.Enums;
using Api.Domain.Shared.Repositories;
using Api.Domain.Species.Models;
using Api.Infrastructure.Data;
using Api.Infrastructure.Middleware.ExceptionHandling;
using Microsoft.EntityFrameworkCore;

namespace Api.Domain.Species.Repositories;

public class TraitRepository(AppDbContext context) : IFeatureRepository<Trait>
{   
    public async Task<Trait> GetByIdAsync(int id) =>
        await context.Traits.FirstOrDefaultAsync(f => f.Id == id)
            ?? throw new NotFoundException($"Trait with id {id} could not be found");

    public async Task<Trait> GetWithAllDataAsync(int id) =>
        await context.Traits
            .AsSplitQuery()
            .Include(t => t.FromRace)
            .Include(f => f.AbilityIncreases)
            .Include(f => f.SpellsGained)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new NotFoundException($"Trait with id {id} could not be found");

    public async Task<Trait> GetWithProficienciesAsync(int id) =>
        await context.Traits
            .Include(f => f.AbilityIncreases)
            .Include(f => f.ArmorProficiencies)
            .Include(f => f.WeaponTypeProficiencies)
            .Include(f => f.SkillProficiencies)
            .Include(f => f.Languages)
            .Include(f => f.ToolProficiencies)
            .Include(f => f.WeaponCategoryProficiencies)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new NotFoundException($"Trait with id {id} could not be found");

    public async Task<Trait> GetWithChoicesAsync(int id) =>
        await context.Traits
            .Include(f => f.AbilityIncreaseChoices)
            .Include(f => f.ArmorProficiencyChoices)
            .Include(f => f.WeaponTypeProficiencyChoices)
            .Include(f => f.SkillProficiencyChoices)
            .Include(f => f.LanguageChoices)
            .Include(f => f.ToolProficiencyChoices)
            .Include(f => f.WeaponCategoryProficiencyChoices)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new NotFoundException($"Trait with id {id} could not be found");

    public async Task<ICollection<Trait>> GetAllAsync() => await context.Traits.ToListAsync();
    
    public async Task<Trait> CreateAsync(Trait entity)
    {
        await context.Traits.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(Trait entity)
    {
        context.Traits.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task<Trait> UpdateAsync(Trait updatedEntity)
    {
        context.Traits.Update(updatedEntity);
        await context.SaveChangesAsync();
        return updatedEntity;
    }

    private readonly Dictionary<string, IEnumerable<Func<Trait, object>>> sortSelectorsMap = new()
    {
        { SortTraitOption.Name, [(t => t.Name)] },
        { SortTraitOption.Race, [(t => t.FromRace!.Name), (t => t.Name)] },
    };
}