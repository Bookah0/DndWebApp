using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Repositories.Backgrounds;
using DndWebApp.Api.Repositories.Features;
using DndWebApp.Api.Services.Generic;
using DndWebApp.Api.Services.Util;

public class BackgroundFeatureService : IService<BackgroundFeature, BackgroundFeatureDto, BackgroundFeatureDto>
{
    protected IBackgroundFeatureRepository repo;
    IBackgroundRepository backgroundRepo;

    public BackgroundFeatureService(IBackgroundFeatureRepository repo, IBackgroundRepository backgroundRepo)
    {
        this.repo = repo;
        this.backgroundRepo = backgroundRepo;
    }

    public async Task<BackgroundFeature> CreateAsync(BackgroundFeatureDto dto)
    {
        ValidationUtil.ValidateRequiredString(dto.Name);
        ValidationUtil.ValidateRequiredString(dto.Description);
        ValidationUtil.ValidateRequiredNumeric(dto.BackgroundId);

        var background = await backgroundRepo.GetByIdAsync(dto.BackgroundId) ?? throw new NullReferenceException("Background could not be found");

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
        var feature = await repo.GetByIdAsync(id) ?? throw new NullReferenceException("Background Feature could not be found");
        await repo.DeleteAsync(feature);
    }

    public async Task<ICollection<BackgroundFeature>> GetAllAsync()
    {
        return await repo.GetAllAsync();
    }

    public async Task<BackgroundFeature> GetByIdAsync(int id)
    {
        return await repo.GetByIdAsync(id) ?? throw new NullReferenceException("Background Feature could not be found");
    }

    public async Task UpdateAsync(BackgroundFeatureDto dto)
    {
        ValidationUtil.ValidateRequiredString(dto.Name);
        ValidationUtil.ValidateRequiredString(dto.Description);

        var feature = await repo.GetByIdAsync(dto.Id) ?? throw new NullReferenceException("Background Feature could not be found");

        if (feature.BackgroundId != dto.BackgroundId)
        {
            feature.Background = await backgroundRepo.GetByIdAsync(dto.BackgroundId) ?? throw new NullReferenceException("Ability could not be found");
            feature.BackgroundId = dto.BackgroundId;
        }

        feature.Name = dto.Name;
        feature.Description = dto.Description;
        feature.IsHomebrew = dto.IsHomebrew;

        await repo.UpdateAsync(feature);
    }

    public async Task UpdateCollectionsAsync(BackgroundFeatureDto dto)
    {
        throw new NotImplementedException();
    }

    public enum BackgroundFeatureSortFilter { Name, Background }
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