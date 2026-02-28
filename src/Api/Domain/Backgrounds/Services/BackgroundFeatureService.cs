using Api.Domain.Backgrounds.Models;
using Api.Domain.Backgrounds.Repositories;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Repositories;
using Api.Domain.Shared.Services;

namespace Api.Domain.Backgrounds.Services;

public class BackgroundFeatureService(
    IFeatureRepository<BackgroundFeature, BackgroundFeatureFilterDto> repo,
    FeatureServiceBaseDependencies dependencies,
    IBackgroundRepository backgroundRepo,
    ILogger<BackgroundFeatureService> logger)
    : FeatureService<BackgroundFeature, CreateBackgroundFeatureRequestDto, UpdateBackgroundFeatureRequestDto, BackgroundFeatureFilterDto>(repo, dependencies, logger)
{
    public async override Task<BackgroundFeature> CreateAsync(CreateBackgroundFeatureRequestDto dto)
    {
        var background = await backgroundRepo.GetByIdAsync(dto.BackgroundId);

        logger.LogInformation("Creating background feature, Name: {BackgroundFeatureName}, BackgroundId: {BackgroundId}", dto.Name, dto.BackgroundId);

        var bgFeature = await repo.CreateAsync(new BackgroundFeature
        {
            Name = dto.Name,
            Description = dto.Description,
            BackgroundId = dto.BackgroundId,
            Background = background,

            CreatedAt = DateTime.UtcNow,
            CreatedBy = dependencies.CurrentUserService.GetCurrentUserId(),
        });

        logger.LogInformation("Successfully created background feature, Name: {BackgroundFeatureName}, ID: {BackgroundFeatureId}", bgFeature.Name, bgFeature.Id);
        return bgFeature;
    }

    public async override Task DeleteAsync(int id)
    {
        var feature = await repo.GetByIdAsync(id);
        logger.LogInformation("Deleting background feature, Name: {BackgroundFeatureName}, ID: {BackgroundFeatureId}", feature.Name, id);
        await repo.DeleteAsync(feature);
        logger.LogInformation("Successfully deleted background feature, Name: {BackgroundFeatureName}, ID: {BackgroundFeatureId}", feature.Name, id);
    }

	public async override Task<ICollection<BackgroundFeature>> GetAllAsync(BackgroundFeatureFilterDto? filter, PaginationRequestDto? pagination) => await repo.GetAllAsync(filter, pagination);
    public async override Task<BackgroundFeature> GetByIdAsync(int id) => await repo.GetByIdAsync(id);

	public async override Task<BackgroundFeature> UpdateAsync(UpdateBackgroundFeatureRequestDto dto, int featureId)
    {
        var feature = await repo.GetByIdAsync(featureId);

        logger.LogInformation("Updating background feature, Name: {BackgroundFeatureName}, ID: {BackgroundFeatureId}", feature.Name, featureId);

        if (dto.NewBackgroundId is not null && feature.BackgroundId != dto.NewBackgroundId)
        {
            feature.Background = await backgroundRepo.GetByIdAsync((int)dto.NewBackgroundId);
            feature.BackgroundId = (int)dto.NewBackgroundId;
        }

        feature.Name = dto.Name ?? feature.Name;
        feature.Description = dto.Description ?? feature.Description;
        feature.IsPublic = dto.IsPublic ?? feature.IsPublic;
        feature.CloningAllowed = dto.CloningAllowed ?? feature.CloningAllowed;
        feature.UpdatedAt = DateTime.UtcNow;

        await repo.UpdateAsync(feature);
        logger.LogInformation("Successfully updated background feature, Name: {BackgroundFeatureName}, ID: {BackgroundFeatureId}", feature.Name, featureId);
        return feature;
    }
}