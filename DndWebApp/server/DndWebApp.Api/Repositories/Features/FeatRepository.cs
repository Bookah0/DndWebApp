using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Features;

public class FeatRepository : IFeatRepository
{
    private readonly AppDbContext context;
    private readonly IRepository<Feat> baseRepo;

    public FeatRepository(AppDbContext context, IRepository<Feat> baseRepo)
    {
        this.context = context;
        this.baseRepo = baseRepo;
    }

    public async Task<Feat> CreateAsync(Feat entity) => await baseRepo.CreateAsync(entity);
    public async Task<Feat?> GetByIdAsync(int id) => await baseRepo.GetByIdAsync(id);
    public async Task<ICollection<Feat>> GetAllAsync() => await baseRepo.GetAllAsync();
    public async Task UpdateAsync(Feat updatedEntity) => await baseRepo.UpdateAsync(updatedEntity);
    public async Task DeleteAsync(Feat entity) => await baseRepo.DeleteAsync(entity);

    public async Task<FeatDto?> GetDtoAsync(int id)
    {
        return await context.Feats
            .AsNoTracking()
            .Select(f => new FeatDto
            {
                Id = f.Id,
                Name = f.Name,
                Description = f.Description,
                IsHomebrew = f.IsHomebrew,
                Prerequisite = f.Prerequisite,
                FromId = f.FromClassId ?? f.FromRaceId ?? f.FromBackgroundId ?? 0,
                FromType =
                    f.FromClassId != null ? FeatFromType.Class :
                    f.FromRaceId != null ? FeatFromType.Race :
                    f.FromBackgroundId != null ? FeatFromType.Background :
                    0
            })
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<FeatDto>> GetDtosAsync()
    {
        return await context.Feats
            .AsNoTracking()
            .Select(f => new FeatDto
            {
                Id = f.Id,
                Name = f.Name,
                Description = f.Description,
                IsHomebrew = f.IsHomebrew,
                Prerequisite = f.Prerequisite,
                FromId = f.FromClassId ?? f.FromRaceId ?? f.FromBackgroundId ?? 0,
                FromType =
                    f.FromClassId != null ? FeatFromType.Class :
                    f.FromRaceId != null ? FeatFromType.Race :
                    f.FromBackgroundId != null ? FeatFromType.Background :
                    0
            })
            .ToListAsync();
    }

    public async Task<Feat?> GetWithAllDataAsync(int id)
    {
        return await context.Feats
            .AsSplitQuery()
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

    public async Task<ICollection<Feat>> GetAllWithAllDataAsync()
    {
        return await context.Feats
            .AsSplitQuery()
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