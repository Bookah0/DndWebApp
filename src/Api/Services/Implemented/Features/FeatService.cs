using Api.Middlewares.ExceptionHandling;
using Api.Models.DTOs.Features;
using Api.Models.Features;
using Api.Repositories.Interfaces;
using Api.Services.Constants;
using Api.Services.Interfaces;
using Api.Services.Interfaces.Features;
using Api.Services.Util;
using static Api.Services.Util.SortUtil;

namespace Api.Services.Implemented.Features;

public class FeatService(
    IFeatureRepository<Feat> repo,
    ISpellRepository spellRepo,
    ISkillRepository skillRepo,
    IAbilityRepository abilityRepo,
    ILanguageRepository languageRepo,
    ICurrentUserService currentUserService,
    ILogger<FeatService> logger)
    : AFeatureService<Feat, CreateFeatRequestDto, UpdateFeatRequestDto>(repo, spellRepo, skillRepo, abilityRepo, languageRepo, logger)
{
    public async override Task<Feat> CreateAsync(CreateFeatRequestDto dto)
    {
        logger.LogInformation("Creating feat, Name: {FeatName}", dto.Name);

        var feat = await repo.CreateAsync(new Feat
        {
            Name = dto.Name,
            Description = dto.Description,

            CreatedAt = DateTime.UtcNow,
            CreatedBy = currentUserService.GetCurrentUserId(),
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

    public async override Task<ICollection<Feat>> GetAllAsync() => await repo.GetAllAsync();
    public async override Task<Feat> GetByIdAsync(int id) => await repo.GetByIdAsync(id);

    public async override Task<Feat> UpdateAsync(UpdateFeatRequestDto dto, int id)
    {
        var feat = await repo.GetByIdAsync(id);
        logger.LogInformation("Updating feat, Name: {FeatName}, ID: {FeatId}", feat.Name, id);

        feat.Name = dto.Name ?? feat.Name;
        feat.Description = dto.Description ?? feat.Description;
        feat.Prerequisite = dto.Prerequisite ?? feat.Prerequisite;

        if(dto.NewFromId is not null)
        {
            if(dto.NewFromType is null)
                throw new ValidationException("NewFromType must be provided when NewFromId is provided.");

            var resolvedSourceType = ConstantsUtil.ResolveOptionOrThrow(dto.NewFromType, FeatSourceConstants.AllowedValues, "Feat source type");
            feat.FromRaceId = resolvedSourceType == FeatSourceConstants.Race || resolvedSourceType == FeatSourceConstants.Subrace ? dto.NewFromId : null;
            feat.FromBackgroundId = resolvedSourceType == FeatSourceConstants.Background ? dto.NewFromId : null;
            feat.FromClassId = resolvedSourceType == FeatSourceConstants.Class || resolvedSourceType == FeatSourceConstants.Subclass ? dto.NewFromId : null;

            if(feat.FromRaceId is null && feat.FromBackgroundId is null && feat.FromClassId is null)
                throw new ValidationException("Invalid NewFromType provided.");
        }

        feat.IsPublic = dto.IsPublic ?? feat.IsPublic;
        feat.CloningAllowed = dto.CloningAllowed ?? feat.CloningAllowed;
        feat.UpdatedAt = DateTime.UtcNow;
        await repo.UpdateAsync(feat);
        logger.LogInformation("Successfully updated feat, Name: {FeatName}, ID: {FeatId}", feat.Name, id);
        
        // refetch to get updated navigation properties, otherwise bg/race/subrace/class/subclass repositories has to be injected into this service
        return await repo.GetByIdAsync(id); 
    }

    public ICollection<Feat> SortBy(ICollection<Feat> feats, bool descending = false)
    {
        return OrderByMany(feats, [(f => f.Name)], descending);
    }
}