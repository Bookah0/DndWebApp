using DndWebApp.Api.Middlewares.ExceptionHandling;
using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.Interfaces.Features;
using DndWebApp.Api.Services.Constants;
using static DndWebApp.Api.Services.Util.SortUtil;
using static DndWebApp.Api.Services.Util.ConstantsUtil;

namespace DndWebApp.Api.Services.Implemented.Features;

public class ClassFeatureService(
    IFeatureRepository<ClassFeature> repo,
    IClassLevelRepository classLevelRepo,
    ISpellRepository spellRepo,
    ISkillRepository skillRepo,
    IAbilityRepository abilityRepo,
    ILanguageRepository languageRepo,
    ILogger<ClassFeatureService> logger)
    : AFeatureService<ClassFeature, ClassFeatureDto>(repo, spellRepo, skillRepo, abilityRepo, languageRepo, logger)
{
    public async override Task<ClassFeature> CreateAsync(ClassFeatureDto dto)
    {
        var level = await classLevelRepo.GetByIdAsync(dto.LevelId) 
            ?? throw new NotFoundException($"Class Level with id {dto.LevelId} could not be found");

        logger.LogInformation("Creating class feature, Name: {ClassFeatureName}, LevelId: {LevelId}", dto.Name, dto.LevelId);

        var classFeature = await repo.CreateAsync(new ClassFeature
        {
            Name = dto.Name,
            Description = dto.Description,
            LevelId = dto.LevelId,
            Level = level,
            ClassId = dto.ClassId,
            IsHomebrew = dto.IsHomebrew
        });

        logger.LogInformation("Successfully created class feature, Name: {ClassFeatureName}, ID: {ClassFeatureId}", classFeature.Name, classFeature.Id);
        return classFeature;
    }

    public async override Task DeleteAsync(int id)
    {
        var feature = await repo.GetByIdAsync(id) 
            ?? throw new NotFoundException($"Class Feature with id {id} could not be found");

        logger.LogInformation("Deleting class feature, Name: {ClassFeatureName}, ID: {ClassFeatureId}", feature.Name, id);
        await repo.DeleteAsync(feature);
        logger.LogInformation("Successfully deleted class feature, Name: {ClassFeatureName}, ID: {ClassFeatureId}", feature.Name, id);
    }

    public async override Task<ICollection<ClassFeature>> GetAllAsync()
    {
        return await repo.GetAllAsync();
    }

    public async override Task<ClassFeature> GetByIdAsync(int id)
    {
        return await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Class Feature with id {id} could not be found");
    }

    public async override Task<ClassFeature> UpdateAsync(ClassFeatureDto dto, int id)
    {
        var feature = await repo.GetByIdAsync(id) 
            ?? throw new NotFoundException($"Class Feature with id {id} could not be found");

        logger.LogInformation("Updating class feature, Name: {ClassFeatureName}, ID: {ClassFeatureId}", feature.Name, id);

        if (feature.LevelId != dto.LevelId)
        {
            feature.Level = await classLevelRepo.GetByIdAsync(dto.LevelId) ?? throw new NotFoundException($"Class Level with id {dto.LevelId} could not be found");
            feature.LevelId = dto.LevelId;
        }

        feature.Name = dto.Name;
        feature.Description = dto.Description;
        feature.IsHomebrew = dto.IsHomebrew;
        await repo.UpdateAsync(feature);
        logger.LogInformation("Successfully updated class feature, Name: {ClassFeatureName}, ID: {ClassFeatureId}", feature.Name, id);
        return feature;
    }

    public ICollection<ClassFeature> SortBy(ICollection<ClassFeature> features, string sortFilter, bool descending = false)
    {
        if(!TryResolveOption(sortFilter, SortClassFeatureOption.AllowedValues, out string? resolved))
            return features;

        return resolved switch
        {
            SortClassFeatureOption.Name => OrderByMany(features, [(l => l.Name)], descending),
            SortClassFeatureOption.Class => OrderByMany(features, [(l => l.Level!.Class.Name), (l => l.Name)], descending),
            _ => throw new ValidationException($"Invalid sort option: {sortFilter}")
        };
    }
}