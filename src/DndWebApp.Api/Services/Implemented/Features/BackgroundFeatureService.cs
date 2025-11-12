
using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.Enums;
using DndWebApp.Api.Services.Interfaces.Features;
using DndWebApp.Api.Services.Util;

namespace DndWebApp.Api.Services.Implemented.Features;

public class BackgroundFeatureService : BaseFeatureService<BackgroundFeature>, IBackgroundFeatureService
{
    private readonly IBackgroundRepository backgroundRepo;

    public BackgroundFeatureService(IBackgroundFeatureRepository repo, IBackgroundRepository backgroundRepo, ISpellRepository spellRepo, ILogger<BackgroundFeatureService> logger) : base(repo, spellRepo, logger)
    {
        this.backgroundRepo = backgroundRepo;
    }

    public async Task<BackgroundFeature> CreateAsync(BackgroundFeatureDto dto)
    {
        ValidationUtil.HasContentOrThrow(dto.Name);
        ValidationUtil.HasContentOrThrow(dto.Description);
        ValidationUtil.AboveZeroOrThrow(dto.BackgroundId);

        var background = await backgroundRepo.GetByIdAsync(dto.BackgroundId) ?? throw new NullReferenceException($"Background with id {dto.BackgroundId} could not be found");

        var bgFeature = new BackgroundFeature
        {
            Name = dto.Name,
            Description = dto.Description,
            BackgroundId = dto.BackgroundId,
            Background = background,
            IsHomebrew = dto.IsHomebrew
        };

        return await repo.CreateAsync(bgFeature);
    }

    public async Task DeleteAsync(int id)
    {
        var feature = await repo.GetByIdAsync(id) ?? throw new NullReferenceException($"Background Feature with id {id} could not be found");
        await repo.DeleteAsync(feature);
    }

    public async Task<ICollection<BackgroundFeature>> GetAllAsync()
    {
        return await repo.GetMiscellaneousItemsAsync();
    }

    public async Task<BackgroundFeature> GetByIdAsync(int id)
    {
        return await repo.GetByIdAsync(id) ?? throw new NullReferenceException($"Background Feature with id {id} could not be found");
    }

    public async Task UpdateAsync(BackgroundFeatureDto dto)
    {
        ValidationUtil.HasContentOrThrow(dto.Name);
        ValidationUtil.HasContentOrThrow(dto.Description);

        var feature = await repo.GetByIdAsync(dto.Id) ?? throw new NullReferenceException($"Background Feature with id {dto.Id} could not be found");

        if (feature.BackgroundId != dto.BackgroundId)
        {
            feature.Background = await backgroundRepo.GetByIdAsync(dto.BackgroundId) ?? throw new NullReferenceException($"Background with id {dto.BackgroundId} could not be found");
            feature.BackgroundId = dto.BackgroundId;
        }

        feature.Name = dto.Name;
        feature.Description = dto.Description;
        feature.IsHomebrew = dto.IsHomebrew;

        await repo.UpdateAsync(feature);
    }

    public ICollection<BackgroundFeature> SortBy(ICollection<BackgroundFeature> features, BackgroundFeatureSortFilter sortFilter, bool descending = false)
    {
        return sortFilter switch
        {
            BackgroundFeatureSortFilter.Name => SortUtil.OrderByMany(features, [(l => l.Name)], descending),
            BackgroundFeatureSortFilter.Background => SortUtil.OrderByMany(features, [(l => l.Background!.Name), (l => l.Name)], descending),
            _ => features,
        };
    }
}