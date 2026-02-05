using Api.Data;
using Api.Middlewares.ExceptionHandling;
using Api.Models.Features;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implemented.Features;

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

    public async Task UpdateAsync(Trait updatedEntity)
    {
        context.Traits.Update(updatedEntity);
        await context.SaveChangesAsync();
    }
}