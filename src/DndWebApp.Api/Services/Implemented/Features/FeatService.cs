using DndWebApp.Api.Middlewares.ExceptionHandling;
using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.Interfaces.Features;
using static DndWebApp.Api.Services.Util.SortUtil;

namespace DndWebApp.Api.Services.Implemented.Features;

public class FeatService(
    IFeatRepository repo,
    ISpellRepository spellRepo,
    ISkillRepository skillRepo,
    IAbilityRepository abilityRepo,
    ILanguageRepository languageRepo,
    ILogger<FeatService> logger)
    : BaseFeatureService<Feat>(repo, spellRepo, skillRepo, abilityRepo, languageRepo, logger), IFeatService
{
    public async Task<Feat> CreateAsync(FeatDto dto)
    {
        var feat = new Feat
        {
            Name = dto.Name,
            Description = dto.Description,
            IsHomebrew = dto.IsHomebrew
        };

        return await repo.CreateAsync(feat);
    }

    public async Task DeleteAsync(int id)
    {
        var feat = await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Feat with id {id} could not be found");
        await repo.DeleteAsync(feat);
    }

    public async Task<ICollection<Feat>> GetAllAsync()
    {
        return await repo.GetAllAsync();
    }

    public async Task<Feat> GetByIdAsync(int id)
    {
        return await repo.GetByIdAsync(id) 
            ?? throw new NotFoundException($"Feat with id {id} could not be found");
    }

    public async Task UpdateAsync(int id, FeatDto dto)
    {
        var feat = await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Feat with id {id} could not be found");

        feat.Name = dto.Name;
        feat.Description = dto.Description;
        await repo.UpdateAsync(feat);
    }

    public Task UpdateCollectionsAsync(int id, FeatDto dto)
    {
        throw new NotImplementedException();
    }

    public ICollection<Feat> SortBy(ICollection<Feat> feats, bool descending = false)
    {
        return OrderByMany(feats, [(f => f.Name)], descending);
    }
}