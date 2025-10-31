using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Features;

public class ClassFeatureRepository : IClassFeatureRepository
{
    private AppDbContext context;
    private IRepository<ClassFeature> baseRepo;

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

    public async Task<ClassFeatureDto?> GetDtoAsync(int id)
    {
        return await context.ClassFeatures
            .AsNoTracking()
            .Select(f => new ClassFeatureDto
            {
                Id = f.Id,
                Name = f.Name,
                Description = f.Description,
                IsHomebrew = f.IsHomebrew,
                ClassLevelId = f.ClassLevelId
            })
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<ClassFeatureDto>> GetAllDtosAsync()
    {
        return await context.ClassFeatures
            .AsNoTracking()
            .Select(f => new ClassFeatureDto
            {
                Id = f.Id,
                Name = f.Name,
                Description = f.Description,
                IsHomebrew = f.IsHomebrew,
                ClassLevelId = f.ClassLevelId
            })
            .ToListAsync();
    }

    public async Task<ClassFeature?> GetWithAllDataAsync(int id)
    {
        return await context.ClassFeatures
            .AsSplitQuery()
            .Include(f => f.ClassLevel)
            .Include(f => f.AbilityIncreases)
            .Include(f => f.SpellsGained)
            .Include(f => f.LanguageChoices)
            .Include(f => f.SkillProficiencyChoices)
            .Include(f => f.ToolProficiencyChoices)
            .Include(f => f.LanguageChoices)
            .Include(f => f.ArmorProficiencyChoices)
            .Include(f => f.WeaponProficiencyChoices)
            .Include(f => f.AbilityIncreaseChoices)
                .ThenInclude(o => o.Options)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<ClassFeature>> GetAllWithAllDataAsync()
    {
        return await context.ClassFeatures
            .AsSplitQuery()
            .Include(f => f.ClassLevel)
            .Include(f => f.AbilityIncreases)
            .Include(f => f.SpellsGained)
            .Include(f => f.LanguageChoices)
            .Include(f => f.SkillProficiencyChoices)
            .Include(f => f.ToolProficiencyChoices)
            .Include(f => f.LanguageChoices)
            .Include(f => f.ArmorProficiencyChoices)
            .Include(f => f.WeaponProficiencyChoices)
            .Include(f => f.AbilityIncreaseChoices)
                .ThenInclude(o => o.Options)
            .ToListAsync();
    }
}