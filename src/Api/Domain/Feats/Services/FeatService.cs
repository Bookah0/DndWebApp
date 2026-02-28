using Api.Domain.Feats.Models;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Repositories;
using Api.Domain.Shared.Services;
using Api.Infrastructure.Middleware.ExceptionHandling;

namespace Api.Domain.Feats.Services;

public class FeatService(
    IFeatureRepository<Feat, FeatFilterDto> repo,
    FeatureServiceBaseDependencies dependencies,
    ILogger<FeatService> logger)
    : FeatureService<Feat, CreateFeatRequestDto, UpdateFeatRequestDto, FeatFilterDto>(repo, dependencies, logger)
{
    public async override Task<Feat> CreateAsync(CreateFeatRequestDto dto)
    {
        ICollection<int?> fromIds = [dto.FromClassId, dto.FromBackgroundId, dto.FromRaceId];
        
        if (fromIds.Count(id => id is not null) > 1)
            throw new ValidationException("Feat can't have more than one source");

        logger.LogInformation("Creating feat, Name: {FeatName}", dto.Name);
        
        var feat = await repo.CreateAsync(new Feat
        {
            Name = dto.Name,
            Description = dto.Description,
            Prerequisite = dto.Prerequisite,
            FromClassId = dto.FromClassId,
            FromRaceId = dto.FromRaceId,
            FromBackgroundId = dto.FromBackgroundId,

            CreatedAt = DateTime.UtcNow,
            CreatedBy = dependencies.CurrentUserService.GetCurrentUserId(),
        });

        logger.LogInformation("Successfully created feat, Name: {FeatName}, ID: {FeatId}", feat.Name, feat.Id);
        return feat;
    }

    public async override Task DeleteAsync(int id)
    {
        var feat = await repo.GetByIdAsync(id);
        logger.LogInformation("Deleting feat, Name: {FeatName}, ID: {FeatId}", feat.Name, id);
        await repo.DeleteAsync(feat);
        logger.LogInformation("Successfully deleted feat, Name: {FeatName}, ID: {FeatId}", feat.Name, id);
    }

    public async override Task<Feat> GetByIdAsync(int id) => await repo.GetByIdAsync(id);

    public async override Task<Feat> UpdateAsync(UpdateFeatRequestDto dto, int id)
    {
        var feat = await repo.GetByIdAsync(id);
        
        logger.LogInformation("Updating feat, Name: {FeatName}, ID: {FeatId}", feat.Name, id);

        feat.Name = dto.Name ?? feat.Name;
        feat.Description = dto.Description ?? feat.Description;
        feat.Prerequisite = dto.Prerequisite ?? feat.Prerequisite;
        UpdateFeatSource(feat, dto);
        
        feat.IsPublic = dto.IsPublic ?? feat.IsPublic;
        feat.CloningAllowed = dto.CloningAllowed ?? feat.CloningAllowed;
        feat.UpdatedAt = DateTime.UtcNow;
        await repo.UpdateAsync(feat);
        logger.LogInformation("Successfully updated feat, Name: {FeatName}, ID: {FeatId}", feat.Name, id);
        
       return await repo.GetByIdAsync(id); 
    }

    private static void UpdateFeatSource(Feat feat, UpdateFeatRequestDto dto)
    {
        ICollection<int?> fromIds = [dto.NewFromClassId, dto.NewFromBackgroundId, dto.NewFromRaceId];
        var fromIdsCount = fromIds.Count(id => id is not null);
        
        if (fromIdsCount > 1)
            throw new ValidationException("Feat can't have more than one source");

        if(fromIdsCount == 1)
        {
            feat.FromBackgroundId = dto.NewFromBackgroundId is not null ? dto.NewFromBackgroundId : null;
            feat.FromClassId = dto.NewFromClassId is not null ? dto.NewFromClassId : null;
            feat.FromRaceId = dto.NewFromRaceId is not null ? dto.NewFromRaceId : null;
        }
    }

	public override Task<ICollection<Feat>> GetAllAsync(FeatFilterDto? filter = null, PaginationRequestDto? pagination = null) 
		=> repo.GetAllAsync(filter, pagination);
}