using Api.Domain.Backgrounds.DTOs;
using Api.Domain.Backgrounds.Models;
using Api.Domain.Backgrounds.Repositories;
using Api.Domain.Characters.Models;
using Api.Domain.Items.Repositories;
using Api.Domain.Shared.Repositories;
using Api.Domain.Users.Services;
using Api.Infrastructure.Middleware.ExceptionHandling;

namespace Api.Domain.Backgrounds.Services;

public class BackgroundService(
    IBackgroundRepository repo, 
    IItemRepository itemRepo, 
    ICurrentUserService currentUserService,
    IFeatureRepository<BackgroundFeature> featureRepo,
    ILogger<BackgroundService> logger) 
    : IBackgroundService
{
    public async Task<Background> CreateAsync(CreateBackgroundRequestDto dto)
    {
        logger.LogInformation("Creating background, Name: {BackgroundName}", dto.Name);

        var StartingCurrency = new Currency
        {
            Brass = dto.StartingCurrency.Brass,
            Copper = dto.StartingCurrency.Copper,
            Electrum = dto.StartingCurrency.Electrum,
            Gold = dto.StartingCurrency.Gold,
            Platinum = dto.StartingCurrency.Platinum,
            Silver = dto.StartingCurrency.Silver
        };
        
        var background = await repo.CreateAsync(new()
        {
            Name = dto.Name,
            Description = dto.Description,
            StartingCurrency = StartingCurrency,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = currentUserService.GetCurrentUserId(),
        });

        logger.LogInformation("Successfully created background, Name: {BackgroundName}, ID: {BackgroundId}", dto.Name, background.Id);

        return background;
    }

    public async Task DeleteAsync(int id)
    {
        var background = await repo.GetByIdAsync(id);
        logger.LogInformation("Deleting background with Name: {BackgroundName}, ID: {BackgroundId}", background.Name, id);
        await repo.DeleteAsync(background);
        logger.LogInformation("Successfully deleted background, Name: {BackgroundName}, ID: {BackgroundId}", background.Name, id);
    }

    public async Task<ICollection<Background>> GetAllAsync() => await repo.GetAllAsync();
    public async Task<Background> GetByIdAsync(int id) => await repo.GetByIdAsync(id);
    public async Task<Background> GetWithAllDataAsync(int id) => await repo.GetWithAllDataAsync(id);
    public async Task<Background> GetWithFeaturesAsync(int id) => await repo.GetWithFeaturesAsync(id);
    public async Task<ICollection<Background>> GetAllWithAllDataAsync() => await repo.GetAllWithAllDataAsync();

    public async Task<Background> UpdateAsync(int id, UpdateBackgroundRequestDto dto)
    {
        var background = await repo.GetByIdAsync(id);

        logger.LogInformation("Updating background, Name: {BackgroundName}, ID: {BackgroundId}", background.Name, id);

        background.Name = dto.Name ?? background.Name;
        background.Description = dto.Description ?? background.Description;
        background.IsPublic = dto.IsPublic ?? background.IsPublic;
        background.CloningAllowed = dto.CloningAllowed ?? background.CloningAllowed; 

        if(dto.StartingCurrency is not null)
        {
            background.StartingCurrency = new Currency
            {
                Brass = dto.StartingCurrency.Brass,
                Copper = dto.StartingCurrency.Copper,
                Electrum = dto.StartingCurrency.Electrum,
                Gold = dto.StartingCurrency.Gold,
                Platinum = dto.StartingCurrency.Platinum,
                Silver = dto.StartingCurrency.Silver
            };
        }

        background.UpdatedAt = DateTime.UtcNow;
        await repo.UpdateAsync(background);
        logger.LogInformation("Successfully updated background, Name: {BackgroundName}, ID: {BackgroundId}", background.Name, id);
        return background;
    }

    public async Task<Background> AddFeatureAsync(int id, int featureId)
    {
        var feature = await featureRepo.GetByIdAsync(featureId);
        var background = await repo.GetByIdAsync(id);

        logger.LogInformation("Adding feature with Name: {FeatureName} ID: {FeatureId} to background, Name: {BackgroundName}, ID: {BackgroundId}", feature.Name, featureId, background.Name, id);
        background.Features.Add(feature);
        
        await repo.UpdateAsync(background);
        logger.LogInformation("Successfully added feature with Name: {FeatureName} ID: {FeatureId} to background, Name: {BackgroundName}, ID: {BackgroundId}", feature.Name, featureId, background.Name, id);
        return background;
    }

    public async Task RemoveFeatureAsync(int id, int featureId)
    {
        var background = await repo.GetByIdAsync(id);

        var feature = background.Features.FirstOrDefault(f => f.Id == featureId) 
            ?? throw new NotFoundException($"Feature with id {featureId} is not a feature for background with id {id}");

        logger.LogInformation("Removing feature with Name: {FeatureName} ID: {FeatureId} from background, Name: {BackgroundName}, ID: {BackgroundId}", feature.Name, featureId, background.Name, id);
        background.Features.Remove(feature);
        await repo.UpdateAsync(background);
        logger.LogInformation("Successfully removed feature with Name: {FeatureName} ID: {FeatureId} from background, Name: {BackgroundName}, ID: {BackgroundId}", feature.Name, featureId, background.Name, id);
    }

    public async Task<Background> AddStartingItemsAsync(int id, int itemId)
    {
        var item = await itemRepo.GetByIdAsync(itemId);
        var background = await repo.GetByIdAsync(id);

        logger.LogInformation("Adding starting item with Name: {ItemName} ID: {ItemId} to background, Name: {BackgroundName}, ID: {BackgroundId}", item.Name, itemId, background.Name, id);
        background.StartingItems.Add(item);
        
        await repo.UpdateAsync(background);
        logger.LogInformation("Successfully added starting item with Name: {ItemName} ID: {ItemId} to background, Name: {BackgroundName}, ID: {BackgroundId}", item.Name, itemId, background.Name, id);
        return background;
    }

    public async Task RemoveStartingItemsAsync(int id, int itemId)
    {
        var background = await repo.GetByIdAsync(id);

        var item = background.StartingItems.FirstOrDefault(i => i.Id == itemId) 
            ?? throw new NotFoundException($"Item with id {itemId} is not a starting item for background with id {id}");

        logger.LogInformation("Removing starting item with Name: {ItemName} ID: {ItemId} from background, Name: {BackgroundName}, ID: {BackgroundId}", item.Name, itemId, background.Name, id);
        background.StartingItems.Remove(item);
        await repo.UpdateAsync(background);
        logger.LogInformation("Successfully removed starting item with Name: {ItemName} ID: {ItemId} from background, Name: {BackgroundName}, ID: {BackgroundId}", item.Name, itemId, background.Name, id);
    }

    public async Task<Background> AddStartingItemChoiceAsync(int id, StartingItemOptionDto dto)
    {
        foreach (var itemId in dto.ItemOptionIds)
        {
            if(!await itemRepo.ExistsAsync(itemId))
                throw new NotFoundException($"Item with id {itemId} could not be found");
        }

        var background = await repo.GetByIdAsync(id);

        logger.LogInformation("Adding starting item option to background, Name: {BackgroundName}, ID: {BackgroundId}", background.Name, id);
        background.StartingItemsOptions.Add(new StartingItemOption
        {
            Description = dto.Description,
            ItemOptionIds = dto.ItemOptionIds,
        });

        await repo.UpdateAsync(background);
        logger.LogInformation("Successfully added starting item option to background, Name: {BackgroundName}, ID: {BackgroundId}", background.Name, id);
        return background;
    }

    public async Task RemoveStartingItemChoiceAsync(int id, int optionId)
    {
        var background = await repo.GetByIdAsync(id);

        var option = background.StartingItemsOptions.FirstOrDefault(o => o.Id == optionId) 
            ?? throw new NotFoundException($"Starting Item Option with id {optionId} is not a starting item option for background with id {id}");

        logger.LogInformation("Removing starting item option with ID: {OptionId} from background, Name: {BackgroundName}, ID: {BackgroundId}", optionId, background.Name, id);
        background.StartingItemsOptions.Remove(option);
        await repo.UpdateAsync(background);
        logger.LogInformation("Successfully removed starting item option with ID: {OptionId} from background, Name: {BackgroundName}, ID: {BackgroundId}", optionId, background.Name, id);
    }
}