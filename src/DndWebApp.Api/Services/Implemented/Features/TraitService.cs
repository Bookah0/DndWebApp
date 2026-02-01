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
    ILogger<TraitService> logger)
    : BaseFeatureService<Trait>(repo, spellRepo, skillRepo, abilityRepo, languageRepo, logger), ITraitService
{
    public async Task<Trait> CreateAsync(TraitDto dto)
    {
           
        var race = await raceRepo.GetByIdAsync(dto.RaceId) ?? throw new NotFoundException($"Trait level with id {dto.RaceId} could not be found");
    
        var trait = new Trait
        {
            Name = dto.Name,
            Description = dto.Description,
            RaceId = dto.RaceId,
            FromRace = race,
            IsHomebrew = dto.IsHomebrew
        };

        return await repo.CreateAsync(trait);
    }

    public async Task DeleteAsync(int traitId)
    {
        var trait = await repo.GetByIdAsync(traitId) 
            ?? throw new NotFoundException($"Trait with id {traitId} could not be found");

        await repo.DeleteAsync(trait);
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

        if (trait.RaceId != dto.RaceId)
        {
            trait.FromRace = await raceRepo.GetByIdAsync(dto.RaceId) ?? throw new NotFoundException($"Race with id {dto.RaceId} could not be found");
            trait.RaceId = dto.RaceId;
        }

        trait.Name = dto.Name;
        trait.Description = dto.Description;
        trait.IsHomebrew = dto.IsHomebrew;
        await repo.UpdateAsync(trait);
    }

    public Task UpdateCollectionsAsync(TraitDto dto, int traitId)
    {
        throw new NotImplementedException();
    }

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