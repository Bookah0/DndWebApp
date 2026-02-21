using Api.Domain.Abilities.Repositories;
using Api.Domain.Languages.Repositories;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Enums;
using Api.Domain.Shared.Repositories;
using Api.Domain.Shared.Services;
using Api.Domain.Shared.Utils;
using Api.Domain.Skills.Repositories;
using Api.Domain.Species.Models;
using Api.Domain.Species.Repositories;
using Api.Domain.Spells.Repositories;
using Api.Domain.Users.Services;
using Api.Infrastructure.Middleware.ExceptionHandling;
using Api.Infrastructure.Validation;

namespace Api.Domain.Species.Services;

public class TraitService(
    IFeatureRepository<Trait> repo,
    IRaceRepository raceRepo,
    ISpellRepository spellRepo,
    ISkillRepository skillRepo,
    IAbilityRepository abilityRepo,
    ILanguageRepository languageRepo,
    ICurrentUserService currentUserService,
    ILogger<TraitService> logger)
    : FeatureService<Trait, CreateTraitRequestDto, UpdateTraitRequestDto>(repo, spellRepo, skillRepo, abilityRepo, languageRepo, logger)
{
    public async override Task<Trait> CreateAsync(CreateTraitRequestDto dto)
    {
        var race = await raceRepo.GetByIdAsync(dto.RaceId);
        logger.LogInformation("Creating trait, Name: {TraitName}, RaceId: {RaceId}", dto.Name, dto.RaceId);    

        Trait trait = await repo.CreateAsync(new() 
        {
            Name = dto.Name,
            Description = dto.Description,
            RaceId = dto.RaceId,
            FromRace = race,

            CreatedAt = DateTime.UtcNow,
            CreatedBy = currentUserService.GetCurrentUserId()
        });

        logger.LogInformation("Successfully created trait, Name: {TraitName}, ID: {TraitId}", trait.Name, trait.Id);
        return trait;
    }

    public async override Task DeleteAsync(int traitId)
    {
        var trait = await repo.GetByIdAsync(traitId);
        logger.LogInformation("Deleting trait, Name: {TraitName}, ID: {TraitId}", trait.Name, traitId);
        await repo.DeleteAsync(trait);
        logger.LogInformation("Successfully deleted trait, Name: {TraitName}, ID: {TraitId}", trait.Name, traitId);
    }

    public async override Task<ICollection<Trait>> GetAllAsync() => await repo.GetAllAsync();
    public async override Task<Trait> GetByIdAsync(int traitId) => await repo.GetByIdAsync(traitId);

    public async override Task<Trait> UpdateAsync(UpdateTraitRequestDto dto, int traitId)
    {
        var trait = await repo.GetByIdAsync(traitId);
        logger.LogInformation("Updating trait, Name: {TraitName}, ID: {TraitId}", trait.Name, traitId);

        if (dto.NewRaceId is not null && trait.RaceId != dto.NewRaceId)
        {
            trait.FromRace = await raceRepo.GetByIdAsync((int)dto.NewRaceId);
            trait.RaceId = (int)dto.NewRaceId;
        }

        trait.Name = dto.Name ?? trait.Name;
        trait.Description = dto.Description ?? trait.Description;
        
        trait.IsPublic = dto.IsPublic ?? trait.IsPublic;
        trait.CloningAllowed = dto.CloningAllowed ?? trait.CloningAllowed;
        trait.UpdatedAt = DateTime.UtcNow;
        await repo.UpdateAsync(trait);
        logger.LogInformation("Successfully updated trait, Name: {TraitName}, ID: {TraitId}", trait.Name, trait.Id);
        return trait;
    }

    // TODO replace with database level sorting
    public ICollection<Trait> SortBy(ICollection<Trait> traits, string sortFilter, bool descending = false)
    {
        if(!ValuesValidator.TryNormalizeValue<SortTraitOption>(sortFilter, out string? normalized))
            return traits;

        return normalized switch
        {
            SortTraitOption.Name => QueryUtil.OrderByMany(traits, [(t => t.Name)], descending),
            SortTraitOption.Race => QueryUtil.OrderByMany(traits, [(t => t!.Name), (t => t.Name)], descending),
            _ => throw new ValidationException($"Invalid sort option: {sortFilter}")
        };
    }
}