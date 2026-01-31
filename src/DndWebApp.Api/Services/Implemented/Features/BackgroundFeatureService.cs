
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

public class BackgroundFeatureService : BaseFeatureService<BackgroundFeature>, IBackgroundFeatureService
{
    private readonly IBackgroundRepository backgroundRepo;

    public BackgroundFeatureService(
        IBackgroundFeatureRepository repo, 
        IBackgroundRepository backgroundRepo, 
        ISpellRepository spellRepo, 
        ISkillRepository skillRepo, 
        IAbilityRepository abilityRepo, 
        ILanguageRepository languageRepo, 
        ILogger<BackgroundFeatureService> logger,
        IMapper mapper) 
        : base(repo, spellRepo, skillRepo, abilityRepo, languageRepo, logger, mapper)
    {
        this.backgroundRepo = backgroundRepo;
    }

    public async Task<BackgroundFeatureResponseDto> CreateAsync(BackgroundFeatureDto dto)
    {
        var background = await backgroundRepo.GetByIdAsync(dto.BackgroundId) ?? throw new NotFoundException($"Background with id {dto.BackgroundId} could not be found");

        var bgFeature = new BackgroundFeature
        {
            Name = dto.Name,
            Description = dto.Description,
            BackgroundId = dto.BackgroundId,
            Background = background,
            IsHomebrew = dto.IsHomebrew
        };

        return mapper.Map<BackgroundFeatureResponseDto>(await repo.CreateAsync(bgFeature));
    }

    public async Task DeleteAsync(int id)
    {
        var feature = await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Background Feature with id {id} could not be found");
        await repo.DeleteAsync(feature);
    }

    public async Task<ICollection<BackgroundFeatureResponseDto>> GetAllAsync()
    {
        return mapper.Map<ICollection<BackgroundFeatureResponseDto>>(await repo.GetAllAsync());
    }

    public async Task<BackgroundFeatureResponseDto> GetByIdAsync(int id)
    {
        return mapper.Map<BackgroundFeatureResponseDto>(await repo.GetByIdAsync(id) 
            ?? throw new NotFoundException($"Background Feature with id {id} could not be found"));
    }

    public async Task UpdateAsync(int id, BackgroundFeatureDto dto)
    {
        var feature = await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Background Feature with id {id} could not be found");

        if (feature.BackgroundId != dto.BackgroundId)
        {
            feature.Background = await backgroundRepo.GetByIdAsync(dto.BackgroundId) ?? throw new NotFoundException($"Background with id {dto.BackgroundId} could not be found");
            feature.BackgroundId = dto.BackgroundId;
        }

        feature.Name = dto.Name;
        feature.Description = dto.Description;
        feature.IsHomebrew = dto.IsHomebrew;

        await repo.UpdateAsync(feature);
    }

    public ICollection<BackgroundFeature> SortBy(ICollection<BackgroundFeature> features, string sortFilter, bool descending = false)
    {
        if(!TryResolveOption(sortFilter, SortBackgroundFeatureOption.AllowedValues, out string? resolved))
            return features;

        return resolved switch
        {
            SortBackgroundFeatureOption.Name => OrderByMany(features, [(l => l.Name)], descending),
            SortBackgroundFeatureOption.Background => OrderByMany(features, [(l => l.Background!.Name), (l => l.Name)], descending),
            _ => throw new ArgumentOutOfRangeException(nameof(sortFilter), "Invalid sort option provided.")
        };
    }
}