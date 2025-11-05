using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.Spells;
using DndWebApp.Api.Repositories.Abilities;
using DndWebApp.Api.Repositories.Features;
using DndWebApp.Api.Repositories.Species;
using DndWebApp.Api.Repositories.Spells;
using DndWebApp.Api.Services.Generic;
using DndWebApp.Api.Services.Util;

namespace DndWebApp.Api.Services.Features;

public class TraitService : BaseFeatureService<Trait>, IService<Trait, TraitDto>
{
    private readonly IRaceRepository raceRepo;

    public TraitService(ITraitRepository repo, IRaceRepository raceRepo, ISpellRepository spellRepo, ILogger<BaseFeatureService<Trait>> logger) : base(repo, spellRepo, logger)
    {
        this.raceRepo = raceRepo;
    }

    public async Task<Trait> CreateAsync(TraitDto dto)
    {
        ValidationUtil.HasContentOrThrow(dto.Name);
        ValidationUtil.HasContentOrThrow(dto.Description);
        ValidationUtil.AboveZeroOrThrow(dto.RaceId);

        var race = await raceRepo.GetByIdAsync(dto.RaceId) ?? throw new NullReferenceException($"Trait level with id {dto.RaceId} could not be found");

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

    public async Task DeleteAsync(int id)
    {
        var trait = await repo.GetByIdAsync(id) ?? throw new NullReferenceException($"Trait with id {id} could not be found");
        await repo.DeleteAsync(trait);
    }

    public async Task<ICollection<Trait>> GetAllAsync()
    {
        return await repo.GetAllAsync();
    }

    public async Task<Trait> GetByIdAsync(int id)
    {
        return await repo.GetByIdAsync(id) ?? throw new NullReferenceException($"Trait with id {id} could not be found");
    }

    public async Task UpdateAsync(TraitDto dto)
    {
        ValidationUtil.HasContentOrThrow(dto.Name);
        ValidationUtil.HasContentOrThrow(dto.Description);

        var trait = await repo.GetByIdAsync(dto.Id) ?? throw new NullReferenceException($"Trait with id {dto.Id} could not be found");

        if (trait.RaceId != dto.RaceId)
        {
            trait.FromRace = await raceRepo.GetByIdAsync(dto.RaceId) ?? throw new NullReferenceException($"Race with id {dto.RaceId} could not be found");
            trait.RaceId = dto.RaceId;
        }

        trait.Name = dto.Name;
        trait.Description = dto.Description;
        trait.IsHomebrew = dto.IsHomebrew;
        await repo.UpdateAsync(trait);
    }

    public Task UpdateCollectionsAsync(TraitDto dto)
    {
        throw new NotImplementedException();
    }

    public enum TraitSortFilter { Name, Race }
    public ICollection<Trait> SortBy(ICollection<Trait> traits, TraitSortFilter sortFilter, bool descending = false)
    {
        return sortFilter switch
        {
            TraitSortFilter.Name => SortUtil.OrderByMany(traits, [(t => t.Name)], descending),
            TraitSortFilter.Race => SortUtil.OrderByMany(traits, [(t => t!.Name), (t => t.Name)], descending),
            _ => traits,
        };
    }
}