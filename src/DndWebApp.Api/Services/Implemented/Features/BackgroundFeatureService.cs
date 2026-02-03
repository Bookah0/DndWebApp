
using DndWebApp.Api.Middlewares.ExceptionHandling;
using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.Interfaces.Features;
using DndWebApp.Api.Services.Constants;
using static DndWebApp.Api.Services.Util.SortUtil;
using static DndWebApp.Api.Services.Util.ConstantsUtil;

namespace DndWebApp.Api.Services.Implemented.Features;

public class BackgroundFeatureService(
    IFeatureRepository<BackgroundFeature> repo,
    IBackgroundRepository backgroundRepo,
    ISpellRepository spellRepo,
    ISkillRepository skillRepo,
    IAbilityRepository abilityRepo,
    ILanguageRepository languageRepo,
    ILogger<BackgroundFeatureService> logger)
    : AFeatureService<BackgroundFeature, BackgroundFeatureDto>(repo, spellRepo, skillRepo, abilityRepo, languageRepo, logger)
{
    public async override Task<BackgroundFeature> CreateAsync(BackgroundFeatureDto dto)
    {
        var background = await backgroundRepo.GetByIdAsync(dto.BackgroundId) 
            ?? throw new NotFoundException($"Background with id {dto.BackgroundId} could not be found");

        logger.LogInformation("Creating background feature, Name: {BackgroundFeatureName}, BackgroundId: {BackgroundId}", dto.Name, dto.BackgroundId);

        var bgFeature = await repo.CreateAsync(new BackgroundFeature
        {
            Name = dto.Name,
            Description = dto.Description,
            BackgroundId = dto.BackgroundId,
            Background = background,
            IsHomebrew = dto.IsHomebrew
        });

        logger.LogInformation("Successfully created background feature, Name: {BackgroundFeatureName}, ID: {BackgroundFeatureId}", bgFeature.Name, bgFeature.Id);
        return bgFeature;
    }

    public async override Task DeleteAsync(int id)
    {
        var feature = await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Background Feature with id {id} could not be found");
        logger.LogInformation("Deleting background feature, Name: {BackgroundFeatureName}, ID: {BackgroundFeatureId}", feature.Name, id);
        await repo.DeleteAsync(feature);
        logger.LogInformation("Successfully deleted background feature, Name: {BackgroundFeatureName}, ID: {BackgroundFeatureId}", feature.Name, id);
    }

    public async override Task<ICollection<BackgroundFeature>> GetAllAsync()
    {
        return await repo.GetAllAsync();
    }

    public async override Task<BackgroundFeature> GetByIdAsync(int id)
    {
        return await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Background Feature with id {id} could not be found");
    }

    public async override Task<BackgroundFeature> UpdateAsync(BackgroundFeatureDto dto, int featureId)
    {
        var feature = await repo.GetByIdAsync(featureId) ?? throw new NotFoundException($"Background Feature with id {featureId} could not be found");

        logger.LogInformation("Updating background feature, Name: {BackgroundFeatureName}, ID: {BackgroundFeatureId}", feature.Name, featureId);

        if (feature.BackgroundId != dto.BackgroundId)
        {
            feature.Background = await backgroundRepo.GetByIdAsync(dto.BackgroundId) ?? throw new NotFoundException($"Background with id {dto.BackgroundId} could not be found");
            feature.BackgroundId = dto.BackgroundId;
        }

        feature.Name = dto.Name;
        feature.Description = dto.Description;
        feature.IsHomebrew = dto.IsHomebrew;

        await repo.UpdateAsync(feature);
        logger.LogInformation("Successfully updated background feature, Name: {BackgroundFeatureName}, ID: {BackgroundFeatureId}", feature.Name, featureId);
        return feature;
    }

    public ICollection<BackgroundFeature> SortBy(ICollection<BackgroundFeature> features, string sortFilter, bool descending = false)
    {
        if(!TryResolveOption(sortFilter, SortBackgroundFeatureOption.AllowedValues, out string? resolved))
            return features;

        return resolved switch
        {
            SortBackgroundFeatureOption.Name => OrderByMany(features, [(l => l.Name)], descending),
            SortBackgroundFeatureOption.Background => OrderByMany(features, [(l => l.Background!.Name), (l => l.Name)], descending),
            _ => throw new ValidationException($"Invalid sort option: {sortFilter}")
        };
    }
}