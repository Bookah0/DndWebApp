using Api.Middlewares.ExceptionHandling;
using Api.Models.DTOs.Features;
using Api.Models.Features;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces.Features;
using Api.Services.Constants;
using static Api.Services.Util.SortUtil;
using static Api.Services.Util.ConstantsUtil;

namespace Api.Services.Implemented.Features;

public class TraitService(
    IFeatureRepository<Trait> repo,
    IRaceRepository raceRepo,
    ISpellRepository spellRepo,
    ISkillRepository skillRepo,
    IAbilityRepository abilityRepo,
    ILanguageRepository languageRepo,
    ILogger<TraitService> logger)
    : AFeatureService<Trait, TraitDto>(repo, spellRepo, skillRepo, abilityRepo, languageRepo, logger)
{
    public async override Task<Trait> CreateAsync(TraitDto dto)
    {
        var race = await raceRepo.GetByIdAsync(dto.RaceId);
        logger.LogInformation("Creating trait, Name: {TraitName}, RaceId: {RaceId}", dto.Name, dto.RaceId);    

        Trait trait = await repo.CreateAsync(new() 
        {
            Name = dto.Name,
            Description = dto.Description,
            RaceId = dto.RaceId,
            FromRace = race,
            IsHomebrew = dto.IsHomebrew
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

    public async override Task<Trait> UpdateAsync(TraitDto dto, int traitId)
    {
        var trait = await repo.GetByIdAsync(traitId);
        logger.LogInformation("Updating trait, Name: {TraitName}, ID: {TraitId}", trait.Name, traitId);

        if (trait.RaceId != dto.RaceId)
        {
            trait.FromRace = await raceRepo.GetByIdAsync(dto.RaceId);
            trait.RaceId = dto.RaceId;
        }

        trait.Name = dto.Name;
        trait.Description = dto.Description;
        trait.IsHomebrew = dto.IsHomebrew;

        await repo.UpdateAsync(trait);
        logger.LogInformation("Successfully updated trait, Name: {TraitName}, ID: {TraitId}", trait.Name, trait.Id);
        return trait;
    }

    // TODO replace with database level sorting
    public ICollection<Trait> SortBy(ICollection<Trait> traits, string sortFilter, bool descending = false)
    {
        if(!TryResolveOption(sortFilter, SortTraitOption.AllowedValues, out string? resolved))
            return traits;

        return resolved switch
        {
            SortTraitOption.Name => OrderByMany(traits, [(t => t.Name)], descending),
            SortTraitOption.Race => OrderByMany(traits, [(t => t!.Name), (t => t.Name)], descending),
            _ => throw new ValidationException($"Invalid sort option: {sortFilter}")
        };
    }
}