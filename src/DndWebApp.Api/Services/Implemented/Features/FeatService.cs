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
        logger.LogInformation("Creating feat, Name: {FeatName}", dto.Name);

        var feat = await repo.CreateAsync(new Feat
        {
            Name = dto.Name,
            Description = dto.Description,
            IsHomebrew = dto.IsHomebrew
        });

        logger.LogInformation("Successfully created feat, Name: {FeatName}, ID: {FeatId}", feat.Name, feat.Id);
        return feat;
    }

    public async Task DeleteAsync(int id)
    {
        var feat = await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Feat with id {id} could not be found");
        logger.LogInformation("Deleting feat, Name: {FeatName}, ID: {FeatId}", feat.Name, id);
        await repo.DeleteAsync(feat);
        logger.LogInformation("Successfully deleted feat, Name: {FeatName}, ID: {FeatId}", feat.Name, id);
    }

    public async Task<ICollection<Feat>> GetAllAsync()
    {
        return await repo.GetAllAsync();
    }

    public async Task<Feat> GetByIdAsync(int id)
    {
        return await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Feat with id {id} could not be found");
    }

    public async Task UpdateAsync(int id, FeatDto dto)
    {
        var feat = await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Feat with id {id} could not be found");
        logger.LogInformation("Updating feat, Name: {FeatName}, ID: {FeatId}", feat.Name, id);

        feat.Name = dto.Name;
        feat.Description = dto.Description;
        await repo.UpdateAsync(feat);
        logger.LogInformation("Successfully updated feat, Name: {FeatName}, ID: {FeatId}", feat.Name, id);
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