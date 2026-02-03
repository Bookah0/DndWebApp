using DndWebApp.Api.Middlewares.ExceptionHandling;
using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.Interfaces.Features;
using DndWebApp.Api.Services.Constants;
using static DndWebApp.Api.Services.Util.SortUtil;
using static DndWebApp.Api.Services.Util.ConstantsUtil;

namespace DndWebApp.Api.Services.Implemented.Features;

public class TraitService(
    ITraitRepository repo,
    IRaceRepository raceRepo,
    ISpellRepository spellRepo,
    ISkillRepository skillRepo,
    IAbilityRepository abilityRepo,
    ILanguageRepository languageRepo,

    IChoiceRepository<AbilityIncreaseChoice> abilityChoiceRepo,
    IChoiceRepository<SkillProficiencyChoice> skillChoiceRepo,
    IChoiceRepository<LanguageChoice> languageChoiceRepo,
    IChoiceRepository<ToolProficiencyChoice> toolChoiceRepo,
    IChoiceRepository<ArmorProficiencyChoice> armorChoiceRepo,
    IChoiceRepository<WeaponCategoryProficiencyChoice> weaponCategoryChoiceRepo,
    IChoiceRepository<WeaponTypeProficiencyChoice> weaponTypeChoiceRepo,

    ILogger<TraitService> logger)
    : AFeatureService<Trait>(repo, spellRepo, skillRepo, abilityRepo, languageRepo, logger), ITraitService
{
    public async Task<Trait> CreateAsync(TraitDto dto)
    {
        var race = await raceRepo.GetByIdAsync(dto.RaceId) ?? throw new NotFoundException($"Trait level with id {dto.RaceId} could not be found");
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

    public async Task DeleteAsync(int traitId)
    {
        var trait = await repo.GetByIdAsync(traitId) 
            ?? throw new NotFoundException($"Trait with id {traitId} could not be found");

        logger.LogInformation("Deleting trait, Name: {TraitName}, ID: {TraitId}", trait.Name, traitId);
        await repo.DeleteAsync(trait);
        logger.LogInformation("Successfully deleted trait, Name: {TraitName}, ID: {TraitId}", trait.Name, traitId);
    }

    public async Task<ICollection<Trait>> GetAllAsync()
    {
        return await repo.GetAllAsync();
    }

    public async Task<Trait> GetByIdAsync(int traitId)
    {
        var trait = await repo.GetByIdAsync(traitId) 
            ?? throw new NotFoundException($"Trait with id {traitId} could not be found");

        return trait;
    }

    public async Task UpdateAsync(TraitDto dto, int traitId)
    {
        var trait = await repo.GetByIdAsync(traitId) ?? throw new NotFoundException($"Trait with id {traitId} could not be found");
        logger.LogInformation("Updating trait, Name: {TraitName}, ID: {TraitId}", trait.Name, traitId);

        if (trait.RaceId != dto.RaceId)
        {
            trait.FromRace = await raceRepo.GetByIdAsync(dto.RaceId) ?? throw new NotFoundException($"Race with id {dto.RaceId} could not be found");
            trait.RaceId = dto.RaceId;
        }

        trait.Name = dto.Name;
        trait.Description = dto.Description;
        trait.IsHomebrew = dto.IsHomebrew;

        await repo.UpdateAsync(trait);
        logger.LogInformation("Successfully updated trait, Name: {TraitName}, ID: {TraitId}", trait.Name, trait.Id);
    }

    public Task UpdateCollectionsAsync(TraitDto dto, int traitId)
    {
        throw new NotImplementedException();
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
            _ => throw new ArgumentOutOfRangeException(nameof(sortFilter), "Invalid sort option provided.")
        };
    }
}