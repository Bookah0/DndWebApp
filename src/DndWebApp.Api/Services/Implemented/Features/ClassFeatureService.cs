using DndWebApp.Api.Middlewares.ExceptionHandling;
using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.Interfaces.Features;
using DndWebApp.Api.Services.Constants;
using static DndWebApp.Api.Services.Util.SortUtil;
using static DndWebApp.Api.Services.Util.ConstantsUtil;

namespace DndWebApp.Api.Services.Implemented.Features;

public class ClassFeatureService : BaseFeatureService<ClassFeature>, IClassFeatureService
{
    private readonly IClassLevelRepository classLevelRepo;

    public ClassFeatureService(
        IClassFeatureRepository repo, 
        IClassLevelRepository classLevelRepo, 
        ISpellRepository spellRepo, 
        ISkillRepository skillRepo, 
        IAbilityRepository abilityRepo, 
        ILanguageRepository languageRepo, 
        ILogger<ClassFeatureService> logger) 
        : base(repo, spellRepo, skillRepo, abilityRepo, languageRepo, logger)
    {
        this.classLevelRepo = classLevelRepo;
    }

    public async Task<ClassFeature> CreateAsync(ClassFeatureDto dto)
    {
        var classLevel = await classLevelRepo.GetByIdAsync(dto.ClassLevelId) ?? throw new NotFoundException($"Class level with id {dto.ClassLevelId} could not be found");

        var classFeature = new ClassFeature
        {
            Name = dto.Name,
            Description = dto.Description,
            ClassLevelId = dto.ClassLevelId,
            ClassLevel = classLevel,
            IsHomebrew = dto.IsHomebrew
        };

        return await repo.CreateAsync(classFeature);
    }

    public async Task DeleteAsync(int id)
    {
        var feature = await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Class Feature with id {id} could not be found");
        await repo.DeleteAsync(feature);
    }

    public async Task<ICollection<ClassFeature>> GetAllAsync()
    {
        return await repo.GetAllAsync();
    }

    public async Task<ClassFeature> GetByIdAsync(int id)
    {
        return await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Class Feature with id {id} could not be found");
    }

    public async Task UpdateAsync(ClassFeatureDto dto)
    {
        var feature = await repo.GetByIdAsync(dto.Id) ?? throw new NotFoundException($"Class Feature with id {dto.Id} could not be found");

        if (feature.ClassLevelId != dto.ClassLevelId)
        {
            feature.ClassLevel = await classLevelRepo.GetByIdAsync(dto.ClassLevelId) ?? throw new NotFoundException($"Class Level with id {dto.ClassLevelId} could not be found");
            feature.ClassLevelId = dto.ClassLevelId;
        }

        feature.Name = dto.Name;
        feature.Description = dto.Description;

        await repo.UpdateAsync(feature);
    }

    public ICollection<ClassFeature> SortBy(ICollection<ClassFeature> features, string sortFilter, bool descending = false)
    {
        if(!TryResolveOption(sortFilter, SortClassFeatureOption.AllowedValues, out string? resolved))
            return features;

        return resolved switch
        {
            SortClassFeatureOption.Name => OrderByMany(features, [(l => l.Name)], descending),
            SortClassFeatureOption.Class => OrderByMany(features, [(l => l.ClassLevel!.Class.Name), (l => l.Name)], descending),
            _ => throw new ArgumentOutOfRangeException(nameof(sortFilter), "Invalid sort option provided.")
        };
    }
}