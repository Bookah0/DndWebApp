using AutoMapper;
using DndWebApp.Api.Middlewares.ExceptionHandling;
using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.DTOs.ResponseDtos;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.Interfaces.Features;
using DndWebApp.Api.Services.Constants;
using static DndWebApp.Api.Services.Util.SortUtil;
using static DndWebApp.Api.Services.Util.ConstantsUtil;

namespace DndWebApp.Api.Services.Implemented.Features;

public class TraitService : BaseFeatureService<Trait>, ITraitService
{
    private readonly IRaceRepository raceRepo;

    public TraitService(
        ITraitRepository repo, 
        IRaceRepository raceRepo, 
        ISpellRepository spellRepo,        
        ISkillRepository skillRepo, 
        IAbilityRepository abilityRepo, 
        ILanguageRepository languageRepo,  
        ILogger<TraitService> logger, 
        IMapper mapper) 
        : base(repo, spellRepo, skillRepo, abilityRepo, languageRepo, logger, mapper)
    {
        this.raceRepo = raceRepo;
    }

    public async Task<TraitResponseDto> CreateAsync(TraitDto dto)
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

        return mapper.Map<TraitResponseDto>(await repo.CreateAsync(trait));
    }

    public async Task DeleteAsync(int id)
    {
        var trait = await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Trait with id {id} could not be found");
        await repo.DeleteAsync(trait);
    }

    public async Task<ICollection<TraitResponseDto>> GetAllAsync()
    {
        return mapper.Map<ICollection<TraitResponseDto>>(await repo.GetAllAsync());
    }

    public async Task<TraitResponseDto> GetByIdAsync(int id)
    {
        return mapper.Map<TraitResponseDto>(await repo.GetByIdAsync(id) 
            ?? throw new NotFoundException($"Trait with id {id} could not be found"));
    }

    public async Task UpdateAsync(int id, TraitDto dto)
    {
        var trait = await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Trait with id {id} could not be found");

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

    public Task UpdateCollectionsAsync(int id, TraitDto dto)
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