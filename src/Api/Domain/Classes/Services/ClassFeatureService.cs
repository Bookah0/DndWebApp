using Api.Domain.Classes.Models;
using Api.Domain.Classes.Repositories;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Repositories;
using Api.Domain.Shared.Services;

namespace Api.Domain.Classes.Services;

public class ClassFeatureService(
    IFeatureRepository<ClassFeature, ClassFeatureFilterDto> repo,
    FeatureServiceBaseDependencies dependencies,
	IClassLevelRepository classLevelRepo,
    ILogger<ClassFeatureService> logger)
    : FeatureService<ClassFeature, CreateClassFeatureRequestDto, UpdateClassFeatureRequestDto, ClassFeatureFilterDto>(repo, dependencies, logger)
{
    public async override Task<ClassFeature> CreateAsync(CreateClassFeatureRequestDto dto)
    {
        var level = await classLevelRepo.GetByIdAsync(dto.LevelId) ;

        logger.LogInformation("Creating class feature, Name: {ClassFeatureName}, LevelId: {LevelId}", dto.Name, dto.LevelId);

        var classFeature = await repo.CreateAsync(new ClassFeature
        {
            Name = dto.Name,
            Description = dto.Description,
            LevelId = dto.LevelId,
            Level = level,
            ClassId = dto.ClassId,

            CreatedAt = DateTime.UtcNow,
            CreatedBy = dependencies.CurrentUserService.GetCurrentUserId(),
        });

        logger.LogInformation("Successfully created class feature, Name: {ClassFeatureName}, ID: {ClassFeatureId}", classFeature.Name, classFeature.Id);
        return classFeature;
    }

    public async override Task DeleteAsync(int id)
    {
        var feature = await repo.GetByIdAsync(id);

        logger.LogInformation("Deleting class feature, Name: {ClassFeatureName}, ID: {ClassFeatureId}", feature.Name, id);
        await repo.DeleteAsync(feature);
        logger.LogInformation("Successfully deleted class feature, Name: {ClassFeatureName}, ID: {ClassFeatureId}", feature.Name, id);
    }

	public async override Task<ICollection<ClassFeature>> GetAllAsync(ClassFeatureFilterDto? filter = null, PaginationRequestDto? pagination = null) => await repo.GetAllAsync(filter, pagination);
    public async override Task<ClassFeature> GetByIdAsync(int id) => await repo.GetByIdAsync(id);

    public async override Task<ClassFeature> UpdateAsync(UpdateClassFeatureRequestDto dto, int id)
    {
        var feature = await repo.GetByIdAsync(id);

        logger.LogInformation("Updating class feature, Name: {ClassFeatureName}, ID: {ClassFeatureId}", feature.Name, id);

        if (dto.NewLevelId is not null && feature.LevelId != dto.NewLevelId)
        {
            feature.Level = await classLevelRepo.GetByIdAsync((int)dto.NewLevelId);
            feature.LevelId = (int)dto.NewLevelId;
        }
        if (dto.NewClassId is not null && feature.ClassId != dto.NewClassId)
        {
            feature.ClassId = (int)dto.NewClassId;
        }

        feature.Name = dto.Name ?? feature.Name;
        feature.Description = dto.Description ?? feature.Description;
        feature.IsPublic = dto.IsPublic ?? feature.IsPublic;
        feature.CloningAllowed = dto.CloningAllowed ?? feature.CloningAllowed;
        feature.UpdatedAt = DateTime.UtcNow;
        await repo.UpdateAsync(feature);
        logger.LogInformation("Successfully updated class feature, Name: {ClassFeatureName}, ID: {ClassFeatureId}", feature.Name, id);
        return feature;
    }
}