using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Repositories.Classes;
using DndWebApp.Api.Repositories.Features;
using DndWebApp.Api.Services.Generic;
using DndWebApp.Api.Services.Util;

namespace DndWebApp.Api.Services.Features;

public class ClassFeatureService : IService<ClassFeature, ClassFeatureDto, ClassFeatureDto>
{
    private readonly IClassFeatureRepository repo;
    private readonly IClassLevelRepository classLevelRepo;
    private readonly ILogger<ClassFeatureService> logger;

    public ClassFeatureService(IClassFeatureRepository repo, IClassLevelRepository classLevelRepo, ILogger<ClassFeatureService> logger)
    {
        this.repo = repo;
        this.classLevelRepo = classLevelRepo;
        this.logger = logger;
    }

    public async Task<ClassFeature> CreateAsync(ClassFeatureDto dto)
    {
        ValidationUtil.NotNullOrWhiteSpace(dto.Name);
        ValidationUtil.NotNullOrWhiteSpace(dto.Description);
        ValidationUtil.NotNullAboveZero(dto.ClassLevelId);

        var classLevel = await classLevelRepo.GetByIdAsync(dto.ClassLevelId) ?? throw new NullReferenceException("Class level could not be found");

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
        var feature = await repo.GetByIdAsync(id) ?? throw new NullReferenceException("Class Feature could not be found");
        await repo.DeleteAsync(feature);
    }

    public async Task<ICollection<ClassFeature>> GetAllAsync()
    {
        return await repo.GetAllAsync();
    }

    public async Task<ClassFeature> GetByIdAsync(int id)
    {
        return await repo.GetByIdAsync(id) ?? throw new NullReferenceException("Class Feature could not be found");
    }

    public async Task UpdateAsync(ClassFeatureDto dto)
    {
        ValidationUtil.NotNullOrWhiteSpace(dto.Name);
        ValidationUtil.NotNullOrWhiteSpace(dto.Description);
        ValidationUtil.NotNullAboveZero(dto.ClassLevelId);

        var feature = await repo.GetByIdAsync(dto.Id) ?? throw new NullReferenceException("Class Feature could not be found");

        if (feature.ClassLevelId != dto.ClassLevelId)
        {
            feature.ClassLevel = await classLevelRepo.GetByIdAsync(dto.ClassLevelId) ?? throw new NullReferenceException("Ability could not be found");
            feature.ClassLevelId = dto.ClassLevelId;
        }

        feature.Name = dto.Name;
        feature.Description = dto.Description;

        await repo.UpdateAsync(feature);
    }

    public Task UpdateCollectionsAsync(ClassFeatureDto dto)
    {
        throw new NotImplementedException();
    }

    public enum ClassFeatureSortFilter { Name, Class }
    public ICollection<ClassFeature> SortBy(ICollection<ClassFeature> features, ClassFeatureSortFilter sortFilter, bool descending = false)
    {
        return sortFilter switch
        {
            ClassFeatureSortFilter.Name => SortUtil.OrderByMany(features, [(l => l.Name)], descending),
            ClassFeatureSortFilter.Class => SortUtil.OrderByMany(features, [(l => l.ClassLevel!.Class.Name), (l => l.Name)], descending),
            _ => features,
        };
    }
}